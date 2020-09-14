using System.Collections.Generic;
using JetBrains.Annotations;

namespace iSukces.Code.Interfaces
{
    public interface IAnnotableByUser
    {
        /// <summary>
        ///     Additional information used by custom generators
        /// </summary>
        [NotNull]
        IDictionary<string, object> UserAnnotations { get; }
    }
}