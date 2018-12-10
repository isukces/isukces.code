using System;
using System.Windows;
using isukces.code.CodeWrite;
using isukces.code.Serenity;
using Xunit;

namespace isukces.code.Tests.Serenity
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



            var file = new CsFile();
            a.Build(file);

            var w = new CodeWriter();
            file.MakeCode(w);
            // var newExpected = Encode(w.Code);
            const string expected = @"// ReSharper disable once CheckNamespace
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

        [DisplayName(""identifier"")]
        [Column(""idx"")]
        [PrimaryKey]
        public int? Id
        {
            get { return Fields.Id[this]; }
            set { Fields.Id[this] = value; }
        }

        [Column(""Name"")]
        [DisplayName(""Name"")]
        [Size(""32"")]
        public string Name
        {
            get { return Fields.Name[this]; }
            set { Fields.Name[this] = value; }
        }

        [Column(""Uid"")]
        [DisplayName(""Uid"")]
        [Serenity.ComponentModel.QuickFilter]
        [NotNull]
        [QuickSearch]
        public Guid? Uid
        {
            get { return Fields.Uid[this]; }
            set { Fields.Uid[this] = value; }
        }

        [Column(""SomeFlag"")]
        [DisplayName(""SomeFlag"")]
        [Serenity.ComponentModel.QuickFilter]
        [NotNull]
        public bool? SomeFlag
        {
            get { return Fields.SomeFlag[this]; }
            set { Fields.SomeFlag[this] = value; }
        }

        [Column(""CreationDate"")]
        [DisplayName(""CreationDate"")]
        [Serenity.ComponentModel.QuickFilter]
        [NotNull]
        public DateTime? CreationDate
        {
            get { return Fields.CreationDate[this]; }
            set { Fields.CreationDate[this] = value; }
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
}