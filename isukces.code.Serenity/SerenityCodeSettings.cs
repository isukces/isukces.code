#nullable disable
using System;

namespace iSukces.Code.Serenity
{
    public static class SerenityCodeSettings
    {        
        /// <summary>
        /// setup this with SerenityCodeSettings.SerenityRowType = typeof(Serenity.Data.Row);
        /// </summary>
        public static Type SerenityRowType { get; set; }

        internal static Type GetSerenityRowType()
        {
            if (SerenityRowType == null)
                throw new NullReferenceException(nameof(SerenityRowType) + " is null");
            return SerenityRowType;
        } 
    }
}
