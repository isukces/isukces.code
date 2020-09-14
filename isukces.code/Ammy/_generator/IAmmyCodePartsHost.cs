using System.Collections.Generic;
using iSukces.Code.Interfaces.Ammy;
using JetBrains.Annotations;

namespace iSukces.Code.Ammy
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