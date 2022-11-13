using System;
using iSukces.Code.Interfaces;
using iSukces.Code.Serenity;
using Xunit;

namespace iSukces.Code.Tests.Serenity
{
    public class SerenityEntityBuilderTests
    {
        [Fact]
        public void T01()
        {
            var a = new SerenityEntityBuilder("Sample", "Cloud", "Common");
            a.WithConnectionKey("Piotr")
                .WithTableName("dbo.Table");


            a.AddProperty<int>("Id")
                .WithDisplayName("identifier")
                .WithColumn("idx")
                .WithIdRow()
                .WithPrimaryKey();
            a.AddStringProperty("Name", 32)
                .WithNameRow();
            a.AddProperty<Guid>("Uid")
                .WithQuickFilter()
                .WithNotNull()
                .WithQuickSearch();


            a.AddProperty<bool>("SomeFlag")
                .WithQuickFilter()
                .WithNotNull();
            
            a.AddProperty<DateTime>("CreationDate")
                .WithQuickFilter()
                .WithNotNull();

            a.AddProperty<SomeEnum32>("Kind32")
                .WithQuickFilter()
                .WithNotNull();
            a.AddProperty<SomeEnum16>("Kind16")
                .WithQuickFilter()
                .WithNotNull()
                .WithFileUpload(aa =>
                {
                    aa.FilenameFormat         = "Documents/~";
                    aa.CopyToHistory          = true;
                    aa.OriginalNameProperty   = "Name";
                    aa.AllowNonImage          = true;
                    aa.DisableDefaultBehavior = true;
                });


            a.AddProperty<short>("MyInt16");
            a.AddProperty<long>("MyInt64");

            var file = new CsFile();
            file.AddImportNamespace(typeof(SomeEnum32));
            a.Build(file);

            ICsCodeWriter w = new CsCodeWriter();
            file.MakeCode(w);
            var newExpected = Encode(w.Code);
          var expected=  @"// ReSharper disable All
using iSukces.Code.Tests.Serenity;

namespace Cloud.Common
{
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;

    [ConnectionKey(""Piotr"")]
    [TableName(""dbo.Table"")]
    public class SampleRow : Row, IIdRow, INameRow
    {
        public SampleRow()
            : (Fields)
        {
        }

        [Column(""idx"")]
        [DisplayName(""identifier"")]
        [PrimaryKey]
        public int? Id
        {
            get { return Fields.Id[this]; }
            set { Fields.Id[this] = value; }
        }

        [Column(""Name"")]
        [DisplayName(""Name"")]
        [Size(32)]
        public string Name
        {
            get { return Fields.Name[this]; }
            set { Fields.Name[this] = value; }
        }

        [Column(""Uid"")]
        [DisplayName(""Uid"")]
        [NotNull]
        [Serenity.ComponentModel.QuickFilter]
        [QuickSearch]
        public Guid? Uid
        {
            get { return Fields.Uid[this]; }
            set { Fields.Uid[this] = value; }
        }

        [Column(""SomeFlag"")]
        [DisplayName(""SomeFlag"")]
        [NotNull]
        [Serenity.ComponentModel.QuickFilter]
        public bool? SomeFlag
        {
            get { return Fields.SomeFlag[this]; }
            set { Fields.SomeFlag[this] = value; }
        }

        [Column(""CreationDate"")]
        [DisplayName(""CreationDate"")]
        [NotNull]
        [Serenity.ComponentModel.QuickFilter]
        public DateTime? CreationDate
        {
            get { return Fields.CreationDate[this]; }
            set { Fields.CreationDate[this] = value; }
        }

        [Column(""Kind32"")]
        [DisplayName(""Kind32"")]
        [NotNull]
        [Serenity.ComponentModel.QuickFilter]
        public SomeEnum32? Kind32
        {
            get { return (SomeEnum32?)Fields.Kind32[this]; }
            set { Fields.Kind32[this] = (int?)value; }
        }

        [Column(""Kind16"")]
        [DisplayName(""Kind16"")]
        [Serenity.ComponentModel.FileUploadEditor(AllowNonImage = true,OriginalNameProperty = ""Name"",CopyToHistory = true,FilenameFormat = ""Documents/~"",DisableDefaultBehavior = true)]
        [NotNull]
        [Serenity.ComponentModel.QuickFilter]
        public SomeEnum16? Kind16
        {
            get { return (SomeEnum16?)Fields.Kind16[this]; }
            set { Fields.Kind16[this] = (short?)value; }
        }

        [Column(""MyInt16"")]
        [DisplayName(""MyInt16"")]
        public short? MyInt16
        {
            get { return Fields.MyInt16[this]; }
            set { Fields.MyInt16[this] = value; }
        }

        [Column(""MyInt64"")]
        [DisplayName(""MyInt64"")]
        public long? MyInt64
        {
            get { return Fields.MyInt64[this]; }
            set { Fields.MyInt64[this] = value; }
        }

        IIdField IIdRow.IdField
        {
            get { return Fields.Id }
        }

        StringField INameRow.IdField
        {
            get { return Fields.Name }
        }

        public readonly RowFields Fields = new RowFields().Init();

        public class RowFields : RowFieldsBase
        {
            public Int32Field Id;

            public StringField Name;

            public GuidField Uid;

            public BooleanField SomeFlag;

            public DateTimeField CreationDate;

            public Int32Field Kind32;

            public Int16Field Kind16;

            public Int16Field MyInt16;

            public Int64Field MyInt64;

        }

    }
}
";
            Assert.Equal(expected, w.Code);
        }

        private static string Encode(string c)
        {
            c = c.Replace("\"", "\"\"");
            c = "@\"" + c + "\"";
            return c;
        }
    }
    public enum SomeEnum32:int {One, Two, Three}
    public enum SomeEnum16:short {One, Two, Three}
}