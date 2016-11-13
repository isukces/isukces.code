using System.Collections.Generic;

namespace isukces.code
{
    public class DictionaryList<TKey, TValue> 
    {

        #region Properties

        /// <summary>
        /// słownik przechowujący
        /// </summary>
        public Dictionary<TKey, List<TValue>> Dictionary { get; set; } = new Dictionary<TKey, List<TValue>>();

        #endregion



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
