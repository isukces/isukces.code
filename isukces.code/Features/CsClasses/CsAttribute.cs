using System;
using System.Collections.Generic;
using System.Linq;
using isukces.code.interfaces;

namespace isukces.code
{
    public class CsAttribute : ClassMemberBase, ICsAttribute
    {
        public CsAttribute(string name)
        {
            Name = name;
        }

        public static implicit operator string(CsAttribute a)
        {
            return a.ToString();
        }

        public static implicit operator CsAttribute(string name)
        {
            return new CsAttribute(name);
        }

        private static string KeyValuePairToString(KeyValuePair<string, string> x)
        {
            return string.IsNullOrEmpty(x.Key) ? x.Value : string.Format("{0} = {1}", x.Key, x.Value);
        }


        public override string ToString()
        {
            var values = _list.Select(KeyValuePairToString).ToArray();
            if (values.Length == 0)
                return Name;
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
            switch (value)
            {
                case null:
                    sValue = "null";
                    break;
                case bool aBool:
                    sValue = aBool ? "true" : "false";
                    break;
                case string aString:
                    sValue = aString.CsEncode();
                    break;
                case IDirectCode aDirectCode:
                    sValue = aDirectCode.Code;
                    break;
                default:
                    throw new NotSupportedException(value.GetType().ToString());
            }
            _list.Add(new KeyValuePair<string, string>(name, sValue));
            return this;
        }


        public string Name { get; set; }


        public string Code
        {
            get
            {
                if (_list == null || _list.Count == 0)
                    return Name;
                return string.Format("{0}({1})",
                    Name,
                    string.Join(",", _list.Select(KeyValuePairToString)));
            }
        }


        private readonly List<KeyValuePair<string, string>> _list = new List<KeyValuePair<string, string>>();
    }
}