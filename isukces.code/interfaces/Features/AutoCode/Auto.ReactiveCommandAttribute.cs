using System;

namespace iSukces.Code.Interfaces;

public static partial class Auto
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ReactiveCommandAttribute : ClassMemberAttributeBase
    {
        public ReactiveCommandAttribute(string name, Type resultType, string? description = null)
        {
            Name        = name;
            ResultType  = resultType;
            Description = description;
        }

        public Type ResultType { get; private set; }

        public Visibilities? SetterVisibility { get; set; }
        public Visibilities? GetterVisibility { get; set; }
    }
}