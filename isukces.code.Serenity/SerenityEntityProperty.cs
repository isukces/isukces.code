using System;
using System.Collections.Generic;
using System.Linq;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Serenity
{
    public class SerenityEntityProperty : IAttributable
    {
        public SerenityEntityProperty(string name, Type type, SerenityEntityBuilder owner)
        {
            Name   = name;
            _owner = owner;
            Type   = new ReflectionTypeWrapper(type);
        }


        public SerenityEntityProperty WithColumn(string id)
        {
            var at = new CsAttribute("Serenity.Data.Mapping.Column").WithArgumentCode(id.CsEncode());
            return AddAttributeOnce(at);
        }


        public SerenityEntityProperty WithDisplayName(string id)
        {
            var at = new CsAttribute("System.ComponentModel.DisplayName").WithArgumentCode(id.CsEncode());
            return AddAttributeOnce(at);
        }

        public SerenityEntityProperty WithFileUpload(Action<FileUploadConfig> action)
        {
            var cfg = new FileUploadConfig();
            action(cfg);
            var at = new CsAttribute("Serenity.ComponentModel.FileUploadEditor");

            void Add(string name, object b)
            {
                if (b == null)
                    return;
                at.WithArgument(name, b);
            }

            Add(nameof(cfg.AllowNonImage), cfg.AllowNonImage);
            Add(nameof(cfg.MaxSize), cfg.MaxSize);
            Add(nameof(cfg.MinSize), cfg.MinSize);
            Add(nameof(cfg.JsonEncodeValue), cfg.JsonEncodeValue);
            Add(nameof(cfg.OriginalNameProperty), cfg.OriginalNameProperty);

            Add(nameof(cfg.DisplayFileName), cfg.DisplayFileName);
            Add(nameof(cfg.CopyToHistory), cfg.CopyToHistory);
            Add(nameof(cfg.FilenameFormat), cfg.FilenameFormat);
            Add(nameof(cfg.DisableDefaultBehavior), cfg.DisableDefaultBehavior);
            Add(nameof(cfg.SubFolder), cfg.SubFolder);

            return AddAttributeOnce(at);
        }

        public SerenityEntityProperty WithIdRow()
        {
            _owner.IdRow = this;
            return this;
        }

        public SerenityEntityProperty WithNameRow()
        {
            _owner.NameRow = this;
            return this;
        }

        public SerenityEntityProperty WithNotNull() => AddAttributeOnce("Serenity.Data.Mapping.NotNull");

        public SerenityEntityProperty WithPrimaryKey() => AddAttributeOnce("Serenity.Data.Mapping.PrimaryKey");

        public SerenityEntityProperty WithQuickFilter() => AddAttributeOnce("Serenity.ComponentModel.QuickFilter");

        public SerenityEntityProperty WithQuickSearch() => AddAttributeOnce("Serenity.Data.Mapping.QuickSearch");

        public SerenityEntityProperty WithSize(int maxLength)
        {
            var at = new CsAttribute("Serenity.Data.Mapping.Size")
                .WithArgument(maxLength);
            return AddAttributeOnce(at);
        }

        private SerenityEntityProperty AddAttributeOnce(CsAttribute at)
        {
            _uniqueAttributes[at.Name] = at;
            return this;
        }

        private SerenityEntityProperty AddAttributeOnce(string className) =>
            AddAttributeOnce(new CsAttribute(className));


        public string Name { get; }

        public IList<ICsAttribute> Attributes { get; } = new List<ICsAttribute>();


        public ReflectionTypeWrapper Type { get; }


        private readonly Dictionary<string, CsAttribute> _uniqueAttributes = new Dictionary<string, CsAttribute>();
        // private readonly CsProperty _property;
        private readonly SerenityEntityBuilder _owner;

        public IEnumerable<ICsAttribute> GetAllAttributes()
        {
            return _uniqueAttributes.Values.OrderBy(a => a.Name.Split('.').Last()).ThenBy(a=>a.Name).Concat(Attributes);
        }
    }

    public class FileUploadConfig
    {
        public bool?  AllowNonImage          { get; set; }
        public int?   MaxSize                { get; set; }
        public int?   MinSize                { get; set; }
        public bool?  JsonEncodeValue        { get; set; }
        public string OriginalNameProperty   { get; set; }
        public bool?  DisplayFileName        { get; set; }
        public bool?  CopyToHistory          { get; set; }
        public string FilenameFormat         { get; set; }
        public bool?  DisableDefaultBehavior { get; set; }
        public string SubFolder              { get; set; }
    }
}