using System;
using iSukces.Code.AutoCode;

namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        private class NestedGeneratorBase
        {
            protected NestedGeneratorBase(IAutoCodeGeneratorContext context, IronyAutocodeGeneratorModel cfg)
            {
                _context = context;
                _cfg     = cfg;
            }

            public static bool SkipCreateClass(NonTerminalInfo info)
            {
                var rule = info.Rule;
                switch (rule)
                {
                    case RuleBuilder.OptionAlternative optionAlternative:
                        if (optionAlternative.Info is TerminalInfo)
                            return true;
                        break;
                }

                return false;
            }

            protected static string MakeList(CsClass cs, string propertyType, Type listType)
            {
                var tmp = cs.GetTypeName(listType).Split('<')[0];
                return $"{tmp}<{propertyType}>";
            }

            protected static CsProperty ProcessProperty(CsProperty prop, bool readOnly)
            {
                prop.WithNoEmitField();
                if (readOnly)
                    prop.WithIsPropertyReadOnly();
                prop.WithMakeAutoImplementIfPossible();
                return prop;
            }


            protected FullTypeName GetFileLevelTypeNameAst(ITypeNameProvider ex)
            {
                var name = ex?.GetTypeName(_context.FileLevelResolver, _cfg.Names.AstNamespace);
                return name;
            }

            protected FullTypeName GetFileLevelTypeNameData(ITypeNameProvider ex)
            {
                var name = ex?.GetTypeName(_context.FileLevelResolver, _cfg.Names.DataNamespace);
                return name;
            }

            protected readonly IAutoCodeGeneratorContext _context;
            protected readonly IronyAutocodeGeneratorModel _cfg;
        }
    }
}