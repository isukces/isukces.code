using System;
using System.Linq;
using System.Xml.Linq;

namespace isukces.code.vssolutions
{
    public abstract class XmlWrapper
    {
        public XmlWrapper(XDocument document) => Document = document;

        protected XElement FindElementByPath(string path)
        {
            var el = Document.Root;
            if (el == null)
                return null;
            var pathElements = path.Split('/').Select(PathElement.FromString).Where(a => a != null).ToArray();
            foreach (var pathElement in pathElements)
            {
                el = el.Element(pathElement.ElementName);
                if (el == null)
                    return null;
            }

            return el;
        }

        protected XElement FindOrCreateElement(string path)
        {
            var el = Document.Root;
            if (el == null)
                throw new NullReferenceException("el");
            var pathElements = path.Split('/').Select(PathElement.FromString).Where(a => a != null).ToArray();
            foreach (var pathElement in pathElements)
            {
                var next = el.Element(pathElement.ElementName);
                if (next == null)
                {
                    next = new XElement(pathElement.ElementName);
                    el.Add(next);
                }

                el = next;
            }

            return el;
        }

        protected string FromElement(string path)
        {
            var el = FindElementByPath(path);
            return el?.Value?.Trim();
        }

        protected void SetValue(string path, string value)
        {
            var el = FindOrCreateElement(path);
            el.Value = value;
        }


        public XDocument Document { get; }
    }
}