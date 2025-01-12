using System;
using iSukces.Code.AutoCode;

#nullable disable
namespace iSukces.Code.Irony
{
    public partial class IronyAutocodeGenerator
    {
        internal class NestedGeneratorBase
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

            protected static string MakeList(CsClass cs, CsType propertyType, Type listType)
            {
#if IGNORECSTYPE
                throw new NotImplementedException();
#else
                var tmp = cs.GetTypeName(listType).BaseName.Split('<')[0];
                return $"{tmp}<{propertyType}>";
#endif
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

