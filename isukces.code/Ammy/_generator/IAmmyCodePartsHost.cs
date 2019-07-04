using System.Collections.Generic;
using isukces.code.interfaces.Ammy;
using JetBrains.Annotations;

namespace isukces.code.Ammy
{
    public static class AmmyCodePartsExtension
    {
        public static void AddMixin(this Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> host, AmmyMixin mixin)
        {
            host[AmmyCodePartsKey.Mixin(mixin.Name)] = mixin;
        }

        public static void AddVariable(this Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> host, string name,
            string value)
        {
            host.AddVariable(new AmmyVariableDefinition(name, value));
        }

        public static void AddVariable(this Dictionary<AmmyCodePartsKey, IAmmyCodePieceConvertible> host,
            AmmyVariableDefinition variableDefinition)
        {
            host[new AmmyCodePartsKey(AmmyCodePartsKeyKind.Variable, variableDefinition.Name)] =
                variableDefinition;
        }
 
    }
}