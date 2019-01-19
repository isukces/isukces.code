// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;
using isukces.code.interfaces.Ammy;

namespace isukces.code.Ammy
{
    public partial class AmmyBind : IAmmyCodePieceConvertible, IComparer<string>,
        IAmmyBindConverterHost, IAmmyBindSourceHost
    {
        public AmmyBind(string bindingPath, DataBindingMode? mode = null)
        {
            BindingPath = bindingPath;
            if (mode != null)
                WithMode(mode);
        }

        public static AmmyBind FromAncestor(string path, Type ancestorType, int? level = null)
        {
            return new AmmyBind(path).WithBindFromAncestor(ancestorType);
        }

        public static AmmyBind FromAncestor<T>(string path, int? level = null)
        {
            return FromAncestor(path, typeof(T), level);
        }

        private static int GetKeywordOrder(string a)
        {
            if (string.IsNullOrEmpty(a)) return 9;
            switch (a)
            {
                case "Mode": return 0;
                case "ValidationRules": return 8;
                case "Converter": return 6;
                default: return 5;
            }
        }

        public int Compare(string x, string y)
        {
            return GetKeywordOrder(x).CompareTo(GetKeywordOrder(y));
        }

        public IAmmyCodePiece ToAmmyCode(IConversionCtx ctx)
        {
            return new SimpleAmmyCodePiece(ToOneLineCode(ctx));
        }

        public string ToOneLineCode(IConversionCtx ctx)
        {
            var txt = new AmmyCodeWriter();
            txt.Append("bind");
            if (!string.IsNullOrEmpty(BindingPath) && BindingPath != ".")
                txt.Append(" " + BindingPath.CsEncode());
            if (From != null)
            {
                var piece = ctx.AnyToCodePiece(From);
                txt.Append(" from ");
                txt.AppendCodePiece(piece);
            }

            var extraSetPartIndent = false;
            if (!SetItems.Any()) return txt.Code;

            var orderedItems    = GetOrderedItems(SetItems);
            var bindingSetItems = new IAmmyCodePiece[orderedItems.Count];
            for (var index = 0; index < bindingSetItems.Length; index++)
            {
                var source = orderedItems[index];
                var code   = ctx.AnyToCodePiece(source.Value);
                code.WriteInSeparateLines = ctx.ResolveSeparateLines("set." + source.Key, code, source.Value, this);
                code                      = code.WithPropertyNameBefore(source.Key);
                bindingSetItems[index]    = code;
            }

            var setCollection = new SetCollection(bindingSetItems);
            var anyInNewLine  = bindingSetItems.Any(a => a.WriteInSeparateLines);
            {
                setCollection.WriteInSeparateLines = anyInNewLine;
                setCollection.WriteInSeparateLines = ctx.ResolveSeparateLines("set", setCollection, null, this);
            }
            anyInNewLine = bindingSetItems.Any(a => a.WriteInSeparateLines);

            if (setCollection.WriteInSeparateLines)
            {
                extraSetPartIndent = true;
                txt.Indent++;
                txt.WriteNewLineAndIndent().Append("set [");
            }
            else
            {
                txt.Append(" set [");
            }

            txt.Indent++;
            {
                // extra indent inside set[] 
                var addComma = false;
                // compact first
                var goToNewLine = anyInNewLine;
                for (var index = 0; index < bindingSetItems.Length; index++)
                {
                    var el = bindingSetItems[index];
                    if (el.WriteInSeparateLines)
                        continue;
                    if (goToNewLine)
                    {
                        goToNewLine = false;
                        txt.WriteNewLineAndIndent();
                    }

                    txt.AppendCommaIf(addComma).AppendCodePiece(el);
                    addComma = true;
                }

                // then multiline
                for (var index = 0; index < bindingSetItems.Length; index++)
                {
                    var el = bindingSetItems[index];
                    if (el.WriteInSeparateLines)
                        txt.WriteNewLineAndIndent().AppendCodePiece(el);
                }
            }
            txt.DecIndent();
            if (setCollection.WriteInSeparateLines)
                txt.WriteNewLineAndIndent();
            txt.Append("]");
            if (extraSetPartIndent)
                txt.Indent--;

            return txt.Code;
        }

        public override string ToString()
        {
            return ToOneLineCode(new ConversionCtx(new AmmyNamespaceProvider()));
        }

        public AmmyBind WithSetParameter(string key, object value)
        {
            if (!string.IsNullOrEmpty(key))
                for (var i = 0; i < SetItems.Count; i++)
                {
                    var pair = SetItems[i];
                    if (pair.Key != key) continue;
                    if (value == null)
                        SetItems.RemoveAt(i);
                    else
                        SetItems[i] = new KeyValuePair<string, object>(key, value);
                    return this;
                }

            if (value != null)
                SetItems.Add(new KeyValuePair<string, object>(key, value));
            return this;
        }

        private List<KeyValuePair<string, object>> GetOrderedItems(IEnumerable<KeyValuePair<string, object>> items)
        {
            return items.OrderBy(a => a.Key, this).ToList();
        }


        void IAmmyBindConverterHost.SetBindConverter(object converter)
        {
            WithSetParameter("Converter", converter);
        }

        void IAmmyBindSourceHost.SetBindingSource(object bindingSource)
        {
            From = bindingSource;
        }

        public string BindingPath { get; set; }

        // public DataBindingMode? Mode        { get; set; }
        public object                             From     { get; set; }
        public List<KeyValuePair<string, object>> SetItems { get; } = new List<KeyValuePair<string, object>>();

        // used only for resolving new lines 
        public class SetCollection : IAmmyCodePiece
        {
            public SetCollection(IAmmyCodePiece[] setItems)
            {
                SetItems = setItems;
            }

            public IReadOnlyList<IAmmyCodePiece> SetItems             { get; }
            public bool                          WriteInSeparateLines { get; set; }
        }
    }

    public enum DataBindingMode
    {
        TwoWay,
        OneWay,
        OneTime,
        OneWayToSource,
        Default
    }
    public enum DataUpdateSourceTrigger
    {
        Default,
        PropertyChanged,
        LostFocus,
        Explicit,
    }
}