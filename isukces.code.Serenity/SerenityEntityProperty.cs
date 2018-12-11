using System;
using System.Collections.Generic;
using System.Globalization;
using isukces.code.interfaces;

namespace isukces.code.Serenity
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
            return this.WithAttribute("Serenity.Data.Mapping.Column", id);
        }


        public SerenityEntityProperty WithDisplayName(string id)
        {
            return this.WithAttribute("System.ComponentModel.DisplayName", id);
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

        public SerenityEntityProperty WithNotNull()
        {
            return this.WithAttribute("Serenity.Data.Mapping.NotNull");
        }

        public SerenityEntityProperty WithPrimaryKey()
        {
            return this.WithAttribute("Serenity.Data.Mapping.PrimaryKey");
        }

        public SerenityEntityProperty WithQuickFilter()
        {
            return this.WithAttribute("Serenity.ComponentModel.QuickFilter");
        }

        public SerenityEntityProperty WithQuickSearch()
        {
            return this.WithAttribute("Serenity.Data.Mapping.QuickSearch");
        }

        public SerenityEntityProperty WithSize(int maxLength)
        {
            return this.WithAttribute("Serenity.Data.Mapping.Size", maxLength.ToString(CultureInfo.InvariantCulture));
        }


        public string Name { get; }

        public IList<ICsAttribute> Attributes { get; } = new List<ICsAttribute>();


        public ReflectionTypeWrapper Type { get; }
        private readonly CsProperty _property;
        private readonly SerenityEntityBuilder _owner;

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

            this.WithAttribute((ICsAttribute)at);
            return this;
        }
    }

    public class FileUploadConfig
    {
        public bool?   AllowNonImage          { get; set; }
        public int?    MaxSize                { get; set; }
        public int?    MinSize                { get; set; }
        public bool?   JsonEncodeValue        { get; set; }
        public string OriginalNameProperty   { get; set; }
        public bool?   DisplayFileName        { get; set; }
        public bool?   CopyToHistory          { get; set; }
        public string FilenameFormat         { get; set; }
        public bool?   DisableDefaultBehavior { get; set; }
        public string SubFolder              { get; set; }
    }
}