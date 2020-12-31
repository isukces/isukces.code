using System;
using JetBrains.Annotations;

namespace iSukces.Code.Irony
{
    public class FullTypeName
    {
        public FullTypeName([NotNull] string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            Name = name.Trim();
        }

        public override string ToString() => Name;

        public string Name { get; }
    }
}