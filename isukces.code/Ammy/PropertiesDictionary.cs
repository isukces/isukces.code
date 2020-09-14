using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iSukces.Code.Ammy
{
    public class PropertiesDictionary : IDictionary<string, object>
    {
        public void Add(KeyValuePair<string, object> item)
        {
            _nested.Add(item);
        }

        public void Add(string key, object value)
        {
            _nested.Add(key, value);
            AddKey(key);
        }

        public void Clear()
        {
            _nested.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return _nested.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _nested.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            _nested.CopyTo(array, arrayIndex);
        }


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var tmp = GetOrderedKeys()
                .Select(a => new KeyValuePair<string, object>(a, _nested[a]));
            return tmp.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            _order.Remove(item.Key);
            return _nested.Remove(item);
        }

        public bool Remove(string key)
        {
            _order.Remove(key);
            return _nested.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return _nested.TryGetValue(key, out value);
        }

        private void AddKey(string key)
        {
            if (_order.ContainsKey(key))
                return;
            _order[key] = ++_max;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_nested).GetEnumerator();
        }

        public int Count => _nested.Count;

        public bool IsReadOnly => _nested.IsReadOnly;

        public ICollection<string> Keys => GetOrderedKeys().ToArray();

        private IEnumerable<string> GetOrderedKeys()
        {
            return _order.OrderBy(a => a.Value).Select(a => a.Key);
        }

        public ICollection<object> Values
        {
            get { return GetOrderedKeys().Select(a => _nested[a]).ToArray(); }
        }

        private readonly IDictionary<string, object> _nested = new Dictionary<string, object>();

        private readonly Dictionary<string, int> _order = new Dictionary<string, int>();

        private int _max;

        public object this[string key]
        {
            get => _nested[key];
            set
            {
                _nested[key] = value;
                AddKey(key);
            }
        }
    }
}