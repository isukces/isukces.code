using System;

namespace iSukces.Code.Interfaces;

public partial class Auto
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ComparerGeneratorAttribute(params string[] fields) : Attribute
    {
        public string[] Fields { get; } = fields;
    }
}