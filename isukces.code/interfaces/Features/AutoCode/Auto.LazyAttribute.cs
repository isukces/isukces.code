using System;

namespace iSukces.Code.Interfaces;

public partial class Auto
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class LazyAttribute : Attribute
    {
        public LazyMemberType Target                     { get; set; } = LazyMemberType.Auto;
        public Visibilities   TargetVisibility           { get; set; } = Visibilities.Public;
        public string?        Name                       { get; set; }
        public string?        SyncObjectName             { get; set; }
        public bool           StaticSyncObject           { get; set; }
        public bool           DeclareAndCreateSyncObject { get; set; } = true;

        public string? FieldName { get; set; }

        public string? ClearMethodName { get; set; }
        /// <summary>
        /// Implement with Lazy&lt;&gt; if possible
        /// </summary>
        public bool UseLazyObject { get; set; } = true;
        
    }
}