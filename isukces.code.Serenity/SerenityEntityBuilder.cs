#nullable disable
using System;
using System.Collections.Generic;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Serenity
{
    public class AttributeHolder : IAttributable
    {
        public IList<ICsAttribute> Attributes { get; } = new List<ICsAttribute>();
    }

    public class SerenityEntityBuilder
    {
        public SerenityEntityBuilder(CsType baseName, string baseNamespace, string moduleName)
        {
            BaseName      = baseName;
            BaseNamespace = baseNamespace;
            ModuleName    = moduleName;
        }

        private static string Cast(string a, string b)
        {
            if (a == b) return "";
            return "(" + b + ")";
        }

        private static string ReduceName(string name, string[] kns)
        {
            if (kns == null || string.IsNullOrEmpty(name))
                return name;
            foreach (var ns in kns)
            {
                var nsLength = ns.Length;
                if (nsLength >= name.Length) continue;
                if (name[nsLength] != '.') continue;
                if (!name.StartsWith(ns)) continue;
                var result = name.Substring(nsLength + 1);
                return result;
            }

            return name;
        }


        public SerenityEntityProperty AddStringProperty(string name, int maxLength=-1)
        {
            var a = AddProperty<string>(name);
            if (maxLength < 1)
                return a;
            return a.WithSize(maxLength);

        }
        public SerenityEntityProperty AddProperty<T>(string name, bool notNull = false)
        {
            var property= AddProperty(name, typeof(T))
                .WithColumn(name)
                .WithDisplayName(name);
            return notNull ? property.WithNotNull() : property;
        }


        public void Build(CsFile file)
        {
            var ns = file.GetOrCreateNamespace(BaseNamespace + "." + ModuleName);
            var kns = new[] // muszą być posortowane wg ilości kropek w nazwie
            {
                "Serenity.Data.Mapping",
                "Serenity.Data",
                "System.ComponentModel",
                "System"
            };
            ns.AddImportNamespace(kns);

            {
                var row = ns.GetOrCreateClass(BaseName.AppendBase("Row"))
                    .WithBaseClass((CsType)"Row");
                var fields = row.GetOrCreateNested((CsType)"RowFields")
                    .WithBaseClass((CsType)"RowFieldsBase");

                row.AddConstructor().BaseConstructorCall = "(Fields)";
                row.AddField("Fields", fields.Name)
                    .WithIsReadOnly()
                    .WithConstValue("new RowFields().Init()");
                
                
                CopyAttributesAndReduceName(_row.Attributes, row, kns);

                foreach (var property in Properties)
                {
                    var types = SerenityTypesTriple.FromType(property.Type.Type);
                    
                    var fieldType    = row.GetTypeName(types.FieldWrapped).Declaration;
                    var propertyType = row.GetTypeName(types.Facade);
                    var p            = row.AddProperty(property.Name, propertyType);
                    p.EmitField = false;
                    p.WithOwnGetterAsExpression($"{Cast(fieldType, propertyType.Declaration)}Fields.{property.Name}[this]");
                    p.WithOwnSetterAsExpression($"Fields.{property.Name}[this] = {Cast(propertyType.Declaration, fieldType)}value");
                    CopyAttributesAndReduceName(property.GetAllAttributes(), p, kns);

                    // var rowFieldType = property.RowFieldType;
                    var f            = fields.AddField(property.Name, types.RowFieldType);
                }

                if (IdRow != null)
                {
                    row.ImplementedInterfaces.Add((CsType)"IIdRow");
                    var p = row.AddProperty("IIdRow.IdField", (CsType)"IIdField")
                        .WithIsPropertyReadOnly()
                        .WithNoEmitField()
                        .WithOwnGetter("return Fields." + IdRow.Name);
                }

                if (NameRow != null)
                {
                    if (NameRow.Type.Type != typeof(string))
                        throw new Exception("Only string column can be used as NameRow");
                    row.ImplementedInterfaces.Add((CsType)"INameRow");
                    var p = row.AddProperty("INameRow.IdField", (CsType)"StringField")
                        .WithIsPropertyReadOnly()
                        .WithNoEmitField()
                        .WithOwnGetter("return Fields." + NameRow.Name);
                }
            }
        }

        private static void CopyAttributesAndReduceName(IEnumerable<ICsAttribute> source, IAttributable target, string[] kns)
        {
            if (source == null)
                return;
            foreach (var attribute in source)
            {
                attribute.Name = ReduceName(attribute.Name, kns);
                target.Attributes.Add(attribute);
            }
        }

        public SerenityEntityBuilder WithConnectionKey(string connectionKey)
        {
            _row.WithAttribute("Serenity.Data.ConnectionKey", connectionKey);
            return this;
        }

        public SerenityEntityBuilder WithTableName(string tableName)
        {
            _row.WithAttribute("Serenity.Data.Mapping.TableName", tableName);
            return this;
        }                

        private SerenityEntityProperty AddProperty(string name, Type type)
        {
            // SerenityEntityProperty
            var property = new SerenityEntityProperty(name, type, this);
            Properties.Add(property);
            return property;
        }

        public CsType                       BaseName      { get; set; }
        public string                       BaseNamespace { get; }
        public string                       ModuleName    { get; }
        public List<SerenityEntityProperty> Properties    { get; } = new List<SerenityEntityProperty>();
        public SerenityEntityProperty       IdRow         { get; set; }
        public SerenityEntityProperty       NameRow       { get; set; }

        private readonly AttributeHolder _row = new AttributeHolder();
    }
}
