#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Interpreter.Ast;
using isukces.code;
using isukces.code.Compatibility.System.Windows.Data;
using X=Bitbrains.AmmyParser.AmmyGrammar;

namespace Bitbrains.AmmyParser
{
    public partial class AmmyGrammarAutogeneratorInfo
    {
        private static IEnumerable<object> GetItemsInternal()
        {
            yield return new AmmyGrammarAutogeneratorInfo("comma_opt")
                .AsOptional(null, "comma");
            yield return new AmmyGrammarAutogeneratorInfo("int_number_optional")
                .AsOptional(null, nameof(X.Number));
            yield return new AmmyGrammarAutogeneratorInfo(nameof(X.boolean))
            {
                BaseClass = nameof(AstNode)
            };
            

            yield return new AmmyGrammarAutogeneratorInfo("literal").AsOneOf();
            yield return "qual_name_segment";
            yield return "qual_name_segments_opt2";
            yield return "qual_name_with_targs";
            yield return "identifier_or_builtin";
            yield return
                new AmmyGrammarAutogeneratorInfo("using_ns_directive").AsAlternative(0); // , "using_ns_directive");
            {
                var el = new AmmyGrammarAutogeneratorInfo("using_directive")
                    .AsOneOf("using_ns_directive");
                // yield return new AmmyGrammarAutogeneratorInfo("using_directives").AsListOf("UsingStatement");
                // yield return "using_directives_opt,AstOptNode,false";

                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }
            yield return new AmmyGrammarAutogeneratorInfo(nameof(X.mixin_definition));

            {
                var el = new AmmyGrammarAutogeneratorInfo("statement")
                    .AsOneOf(
                        nameof(X.mixin_definition)
                    );

                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }
            {
                var el = new AmmyGrammarAutogeneratorInfo("object_setting")
                    .AsOneOf("object_property_setting");
                var list = el.GetList();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }

            {
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind");
                var el = new AmmyGrammarAutogeneratorInfo("ammy_bind_source").Single();
                yield return el;
                yield return el.GetOptional();

                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_source")
                    .AsAlternative(0,
                        "ammy_bind_source_ancestor",
                        "ammy_bind_source_element_name",
                        "ammy_bind_source_this").Single();
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_ancestor");
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_element_name").Single();
                yield return new AmmyGrammarAutogeneratorInfo("ammy_bind_source_this");
                {
                    var set = new AmmyGrammarAutogeneratorInfo("ammy_bind_set");
                    yield return set;
                    yield return set.GetOptional();
                }
                {
                    var item = new AmmyGrammarAutogeneratorInfo("ammy_bind_set_item")
                        .WithMap(0);
                    var bindingSettings = GetBindingSettings()
                        .Select(a => a.ToArray()).ToArray();
                    foreach (var i in bindingSettings)
                    {
                        foreach (var j in i)
                            yield return j;
                    }

                    var alts = bindingSettings.Select(a => a.Last().TerminalName);
                    item.Alternatives = alts.ToArray();
                    var list = item.GetList(true);
                    yield return item;
                    yield return list;
                    yield return list.GetOptional();
                }
            }
            yield return new AmmyGrammarAutogeneratorInfo("object_property_setting");
            yield return new AmmyGrammarAutogeneratorInfo("ammy_property_name").AsOneOf("identifier");
            yield return new AmmyGrammarAutogeneratorInfo("ammy_property_value")
                .AsOneOf("primary_expression", "ammy_bind");

            yield return new AmmyGrammarAutogeneratorInfo("primary_expression").AsOneOf("literal");
            yield return new AmmyGrammarAutogeneratorInfo("expression").AsOneOf("primary_expression");

            {
                var el = new AmmyGrammarAutogeneratorInfo("mixin_or_alias_argument").AsOneOf("identifier");
                //var list = el.GetList();
                var list = new AmmyGrammarAutogeneratorInfo(el.TerminalName + "s").AsListOf<object>();
                yield return el;
                yield return list;
                yield return list.GetOptional();
            }

            yield return new AmmyGrammarAutogeneratorInfo("ammyCode")
                .AsListOf<object>();
        }


        public static AmmyGrammarAutogeneratorInfo[] Items()
        {
            var nt = GetItemsInternal().Select(a =>
            {
                switch (a)
                {
                    case string s:
                        return new AmmyGrammarAutogeneratorInfo(s);
                    case AmmyGrammarAutogeneratorInfo i:
                        return i;
                    default:
                        throw new NotSupportedException();
                }
            }).ToArray();
            return nt;
        }

        private static IEnumerable<IEnumerable<AmmyGrammarAutogeneratorInfo>> GetBindingSettings()
        {
            string GetValueToken(Type t)
            {
                if (t == typeof(string))
                    return nameof(X.TheStringLiteral);
                if (t == typeof(bool))
                    return nameof(X.boolean);
                if (t == typeof(int))
                    return nameof(X.Number);
               
                return null;
            }

            IEnumerable<AmmyGrammarAutogeneratorInfo> Build<T>(string name, string rule=null)
            {
                var type = typeof(T);
                var el = new AmmyGrammarAutogeneratorInfo("ammy_bind_set_" + name);
                var valueToken = GetValueToken(type);
                if (valueToken is null)
                {
                    if (type.IsEnum)
                    {
                        var names = Enum.GetNames(type);
                        var items = names.Select(a => "ToTerm(" + a.CsEncode() + ")");
                        var enums = new AmmyGrammarAutogeneratorInfo("ammy_bind_set_" + name + "_enum_values")
                        {
                            BaseClass = nameof(EnumValueNode), 
                            Rule = string.Join(" | ", items)
                        };
                        yield return enums;
                        valueToken = enums.TerminalName;
                    }
                }


                el.Rule = rule;
                if (valueToken != null)
                {
                    var a = new[]
                    {
                        $"ToTerm({name.CsEncode()})",
                        ":".CsEncode(),
                        valueToken,
                        nameof(X.comma_opt)
                    };
                    el.Rule = string.Join(" + ", a);
                    el.Punctuations.Add(name);
                    el.Punctuations.Add(":");
                    el.Map = new[] {0};
                    el.Keyword = name;
                } 

                yield return el;
            }

            /*yield return Build<string>("BindingGroupName");
            yield return Build<bool>("BindsDirectlyToSource");
            yield return Build<object>("Mode", "");
            */

            yield return Build<XBindingMode>("Mode");
            // yield return Build<object>("Converter");
            // yield return Build<object>("ConverterCulture");
            // yield return Build<object>("ConverterParameter");
            yield return Build<bool>("NotifyOnSourceUpdated");
            yield return Build<bool>("NotifyOnTargetUpdated");
            yield return Build<bool>("NotifyOnValidationError");

            yield return Build<bool>("ValidatesOnExceptions");
            yield return Build<bool>("ValidatesOnDataErrors");
            yield return Build<bool>("ValidatesOnNotifyDataErrors");
            yield return Build<string>("StringFormat");
            yield return Build<string>("BindingGroupName");
            yield return Build<int>("FallbackValue");
            yield return Build<bool>("IsAsync");

            // yield return Build<XUpdateSourceTrigger>("UpdateSourceTrigger");
            // yield return Build<object>(ValidationRules);
            // yield return Build<object>("TargetNullValue");            
        }
    }
}
#endif