#nullable enable
using System.Collections.Generic;

namespace iSukces.Code
{
    public class DictionaryList<TKey, TValue> 
    {
        /// <summary>
        /// słownik przechowujący
        /// </summary>
        public Dictionary<TKey, List<TValue>> Dictionary { get; set; } = new Dictionary<TKey, List<TValue>>();


        public void AddItem(TKey key, TValue value)  {
            List<TValue> l;
            if (!Dictionary.TryGetValue(key, out l))
            {
                l = new List<TValue>();
                Dictionary[key] = l; 
            }
            if (!l.Contains(value))
                l.Add(value);
        }
    }
}
