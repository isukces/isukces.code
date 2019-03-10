using System;

namespace isukces.code.interfaces
{
    public enum Visibilities
    {
        Public,
        Protected,
        Private,
        InterfaceDefault,
        Internal
    }

    public static class VisibilitiesExtensions
    {
        public static string ToCsCode(this Visibilities visibility)
        {
            switch (visibility)
            {
                case Visibilities.Public:
                    return "public";
                case Visibilities.Protected:
                    return "protected";
                case Visibilities.Private:
                    return "private";
                case Visibilities.Internal:
                    return "internal";
                case Visibilities.InterfaceDefault:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}