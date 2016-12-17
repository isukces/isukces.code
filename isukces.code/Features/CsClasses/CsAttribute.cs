#region using

using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

#endregion

namespace isukces.code
{
    public class CsAttribute : ClassMemberBase, ICsAttribute
    {
        #region Static Methods

        public static implicit operator string(CsAttribute a)
        {
            return a.ToString();
        }

        private static string KeyValuePairToString(KeyValuePair<string, string> x)
        {
            return string.IsNullOrEmpty(x.Key) ? x.Value : string.Format("{0} = {1}", x.Key, x.Value);
        }

        #endregion

        #region Instance Methods

        public override string ToString()
        {
            var values = _list.Select(KeyValuePairToString);
            return string.Format("{0}({1})", Name, string.Join(", ", values));
            // [PrimaryKey("Id", autoIncrement = false)]
        }

        public CsAttribute WithArgument(object value)
        {
            return WithArgument("", value);
        }

        public CsAttribute WithArgument(string name, object value)
        {
            name = (name ?? "").Trim();
            string sValue;
            if (value == null)
                sValue = "null";
            else if (value is bool)
                sValue = (bool)value ? "true" : "false";
            else if (value is string)
                sValue = ((string)value).CsharpCite();
            else
                throw new NotSupportedException(value.GetType().ToString());
            _list.Add(new KeyValuePair<string, string>(name, sValue));
            return this;
        }

        #endregion

        #region Properties

        public string Name { get; set; }


        public string Code
        {
            get
            {
                if ((_list == null) || (_list.Count == 0))
                    return Name;
                return string.Format("{0}({1})",
                    Name,
                    string.Join(",", _list.Select(KeyValuePairToString)));
            }
        }

        #endregion

        #region Fields

        private readonly List<KeyValuePair<string, string>> _list = new List<KeyValuePair<string, string>>();

        #endregion
    }
}