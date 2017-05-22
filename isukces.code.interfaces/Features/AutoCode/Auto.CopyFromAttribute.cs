using System;
using System.Collections.Generic;
using System.Linq;

namespace isukces.code.interfaces
{
    public partial class Auto
    {
        [AttributeUsage(AttributeTargets.Class)]
        public class CopyFromAttribute : Attribute
        {
            #region Properties

            public string CopyByReference
            {
                get { return _copyByReference; }
                set
                {
                    _copyByReference = value;
                    _copyByReferenceHash = new HashSet<string>();
                    var items = (value ?? "").Split(',').Select(a => a.Trim()).Where(i => !string.IsNullOrEmpty(i));
                    foreach (var i in items)
                        _copyByReferenceHash.Add(i);
                }
            }


            public string Skip
            {
                get { return _skip; }
                set
                {
                    _skip = value;
                    _skipHash = new HashSet<string>();
                    var items = (value ?? "").Split(',').Select(a => a.Trim()).Where(i => !string.IsNullOrEmpty(i));
                    foreach (var i in items)
                        _skipHash.Add(i);
                }
            }

            #endregion

            #region Fields

            private string _copyByReference;
            private HashSet<string> _copyByReferenceHash;
            private string _skip;
            private HashSet<string> _skipHash;

            #endregion

            #region Methods 

            // Public Methods 

            public bool HasCopyByReference(string name)
            {
                return (_copyByReferenceHash != null) && _copyByReferenceHash.Contains(name);
            }


            public bool HasSkip(string name)
            {
                return (_skipHash != null) && _skipHash.Contains(name);
            }

            #endregion Methods 
        }
    }
}