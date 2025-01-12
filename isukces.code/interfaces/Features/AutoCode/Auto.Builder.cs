using System;

namespace iSukces.Code.Interfaces
{
    public partial class Auto
    {
        public class BuilderAttribute : Attribute
        {
            public string BuilderClassName { get; set; }
        }
    }
}
