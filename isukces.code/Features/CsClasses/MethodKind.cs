namespace iSukces.Code
{
    public enum MethodKind
    {
        Normal,
        Constructor,
        Finalizer,
        Operator,

        // np.  public static implicit operator double(Force src)
        Implicit,
        Explicit,
    }

    public enum StaticInstanceStatus
    {
        Static,
        Instance,
        Both
    }

    public static class MethodKindExtensions
    {
        public static StaticInstanceStatus GetStaticInstanceStatus(this MethodKind kind)
        {
            switch (kind)
            {
                case MethodKind.Finalizer:
                    return StaticInstanceStatus.Instance;
                case MethodKind.Normal:
                case MethodKind.Constructor:
                    return StaticInstanceStatus.Both;
                default:
                    return StaticInstanceStatus.Static;
            }
        }
    }
}