using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Ammy
{
    public class AmmyCallBuilder
    {
        public AmmyCallBuilder(string name = null)
        {
            Name = name;
        }

        public static SimpleAmmyCodePiece BuildAliasCall(string name, params string[] args)
        {
            return Call("@" + name, args);
        }

        public static SimpleAmmyCodePiece BuildMixinCall(string name, params string[] args)
        {
            return Call("#" + name, args);
        }

        public static SimpleAmmyCodePiece Call(string name, params string[] args)
        {
            var code = string.Format("{0}({1})", name, string.Join(",", args));
            return new SimpleAmmyCodePiece(code);
        }

        public SimpleAmmyCodePiece BuildAliasCall()
        {
            return BuildAliasCall(Name, Arguments.ToArray());
        }
        
        public SimpleAmmyCodePiece BuildMixinCall()
        {
            return BuildMixinCall(Name, Arguments.ToArray());
        }

        public AmmyCallBuilder WithTextArgument(string x, AmmyCallBuilderOption option = AmmyCallBuilderOption.None)
        {
            if (x == null)
                x = option == AmmyCallBuilderOption.NullToNone ? "none" : "null";
            else
                x = x.CsEncode();
            Arguments.Add(x);
            return this;
        }
        
        public AmmyCallBuilder WithAmmyArgument(string x, AmmyCallBuilderOption option = AmmyCallBuilderOption.None)
        {
            if (x == null)
                x = option == AmmyCallBuilderOption.NullToNone ? "none" : "null";
            Arguments.Add(x);
            return this;
        }

        public AmmyCallBuilder WithTrimLastNones()
        {
            while (Arguments.LastOrDefault() == "none")
                Arguments.RemoveAt(Arguments.Count - 1);

            return this;
        }

        public List<string> Arguments { get; } = new List<string>();


        public string Name { get; set; }
    }

    public enum AmmyCallBuilderOption
    {
        None,
        NullToNone
    }
}