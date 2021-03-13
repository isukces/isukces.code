using System;

namespace iSukces.Code
{
    partial class CsMethod
    {
        [Obsolete("Use add comment")]
        public string AdditionalContentOverMethod { get; set; }

        /// <summary>
        /// </summary>
        [Obsolete("Use " + nameof(Overriding) + " instead")]
        public bool IsAbstract
        {
            get => Overriding == OverridingType.Abstract;
            set => Overriding = value ? OverridingType.Abstract : OverridingType.None;
        }

        /// <summary>
        /// </summary>
        [Obsolete("Use " + nameof(Overriding) + " instead")]
        public bool IsOverride
        {
            get => Overriding == OverridingType.Override;
            set => Overriding = value ? OverridingType.Override : OverridingType.None;
        }

    }
}