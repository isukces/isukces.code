using System;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Interfaces;

public partial class Auto
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CopyFromAttribute : Attribute
    {

        public bool HasCopyByReference(string name) =>
            _copyByReferenceHash is not null && _copyByReferenceHash.Contains(name);


        public bool HasSkip(string name) => _skipHash is not null && _skipHash.Contains(name);

        public string CopyByReference
        {
            get => _copyByReference;
            set
            {
                _copyByReference     = value;
                _copyByReferenceHash = new HashSet<string>();
                var items = (value ?? "").Split(',').Select(a => a.Trim()).Where(i => !string.IsNullOrEmpty(i));
                foreach (var i in items)
                    _copyByReferenceHash.Add(i);
            }
        }


        public string Skip
        {
            get => _skip;
            set
            {
                _skip     = value;
                _skipHash = new HashSet<string>();
                var items = (value ?? "").Split(',').Select(a => a.Trim()).Where(i => !string.IsNullOrEmpty(i));
                foreach (var i in items)
                    _skipHash.Add(i);
            }
        }

        private string          _copyByReference;
        private HashSet<string> _copyByReferenceHash;
        private string          _skip;
        private HashSet<string> _skipHash;
    }
}
