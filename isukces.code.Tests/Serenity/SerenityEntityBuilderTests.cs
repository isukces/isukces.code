using System;
using isukces.code.CodeWrite;
using isukces.code.interfaces;
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
                //.WithIdRow("Id")  // IIdField IdField { get; }
                // WithNameRow("Name")

            a.AddProperty<int>("Id")
                .WithDisplayName("identifier")
                .WithColumn("idx")
                .WithIdRow()
                .WithPrimaryKey();
            a.AddProperty<string>("Name")
                .WithNameRow();
            
            var file = new CsFile();
            a.Build(file);

            var w = new CodeWriter(); 
            file.MakeCode(w);
            var expected = @"// ReSharper disable once CheckNamespace
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
        public string Name
        {
            get { return Fields.Name[this]; }
            set { Fields.Name[this] = value; }
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

        }

    }
}
";
            Assert.Equal(expected, w.Code);
        }
    }
}