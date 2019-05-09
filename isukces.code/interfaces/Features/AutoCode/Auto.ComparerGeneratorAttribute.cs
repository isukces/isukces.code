using System;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
        public class ComparerGeneratorAttribute : Attribute
        {
            public ComparerGeneratorAttribute(params string[] fields)
            {
                Fields = fields;
            }

            public string[] Fields { get; }
        }
    }
}