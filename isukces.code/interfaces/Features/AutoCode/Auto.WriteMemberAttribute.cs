using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class WriteMemberAttribute : Attribute
        {
            public WriteMemberAttribute(string name)
            {
                Name = name;
            }

            public string Name { get; }
        }
    }
}
