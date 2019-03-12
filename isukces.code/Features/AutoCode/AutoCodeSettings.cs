using System;

namespace isukces.code
{
    public class AutoCodeSettings
    {
        public StaticMethodInfo GetUrlStringEncodeOrThrow()
        {
            var tmp = UrlStringEncode;
            if (tmp.IsEmpty)
                throw new Exception(nameof(UrlStringEncode) + " is empty");
            return tmp;
        }

        public static AutoCodeSettings Default => InstanceHolder.DefaultInstance;

        public StaticMethodInfo UrlStringEncode { get; set; }


        private class InstanceHolder
        {
            public static readonly AutoCodeSettings DefaultInstance = new AutoCodeSettings();
        }
    }
}