using isukces.code.Typescript;
using Xunit;

namespace isukces.code.Tests
{
    public class TsTests
    {
        private static string GetCode(ITsCodeProvider ns)
        {
            var cf = new CSCodeFormatter();
            ns.WriteCodeTo(new TsWriteContext(cf));
            return cf.Text.Trim();
        }

        [Fact]
        public void T01_Should_create_empty_class()
        {
            var c = new TsClass("SampleClass");
            var code = GetCode(c);
            var expected = @"
class SampleClass
{
}";
            Assert.Equal(expected.Trim(), code);

            c.Extends = "MyBaseClass";
            code = GetCode(c);
            expected = @"
class SampleClass extends MyBaseClass
{
}";
            Assert.Equal(expected.Trim(), code);

            c.IsExported = true;
            code = GetCode(c);
            expected = @"
export class SampleClass extends MyBaseClass
{
}";
            Assert.Equal(expected.Trim(), code);
        }

        [Fact]
        public void T02_Should_create_namespace_with_empty_class()
        {
            var tsNamespace = new TsNamespace("Sample.Tests");
            tsNamespace.AddClass("SampleClass");
            var code = GetCode(tsNamespace);
            var expected = @"
namespace Sample.Tests
{
    class SampleClass
    {
    }
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T03_Should_create_class_with_method()
        {
            var c = new TsClass("SampleClass");
            var m = c.AddMethod("getValue");
            m.IsStatic = true;
            m.ResultType = "number";
            m.WithArgument("id", "string");

            var code = GetCode(c);
            var expected = @"
class SampleClass
{
    static getValue(id: string) : number
    {
    }
}";
            Assert.Equal(expected.Trim(), code);

            m.Body = "return id + 1;";
            code = GetCode(c);
            expected = @"
class SampleClass
{
    static getValue(id: string) : number
    {
        return id + 1;
    }
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T04_Should_crate_class_with_decoration()
        {
            var tsClass = new TsClass("Something");
            tsClass.AddDecorator("Decorator");
            var code = GetCode(tsClass);
            var expected = @"
@Decorator()
class Something
{
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T05_Should_create_field()
        {
            var tsClass = new TsClass("Something");
            var f = tsClass.AddField("myValue");
            f.IsStatic = true;

            var code = GetCode(tsClass);
            var expected = @"
class Something
{
    static myValue;
}";
            Assert.Equal(expected.Trim(), code);

            f.Initializer = "7";
            code = GetCode(tsClass);
            expected = @"
class Something
{
    static myValue = 7;
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T06_Should_create_interface_with_method()
        {
            var c = new TsInterface("SampleInterface");
            var m = c.AddMethod("getValue");
            m.IsStatic = true;
            m.ResultType = "number";
            m.WithArgument("id", "string");
            m.Body = "return 1;";

            var code = GetCode(c);
            var expected = @"
interface SampleInterface
{
    static getValue(id: string) : number;
}";
            Assert.Equal(expected.Trim(), code);

            m.Body = "return id + 1;";
            code = GetCode(c);
            expected = @"
interface SampleInterface
{
    static getValue(id: string) : number;
}";
            Assert.Equal(expected.Trim(), code);

            var f = c.AddField("value");
            f.Type = "number";
            f.Initializer = "7";
            code = GetCode(c);
            expected = @"
interface SampleInterface
{
    static getValue(id: string) : number;
    value: number;
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T07_Should_create_exported_namespace()
        {
            var ns = new TsNamespace("Namespace") { IsExport = true };
            var code = GetCode(ns);
            var expected = @"
export namespace Namespace
{
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T08_Should_write_direct_code()
        {
            var f = new TsFile();
            f.Members.Add(new TsDirectCode("var i = 1;"));
            var code = GetCode(f);
            var expected = "var i = 1;";
            Assert.Equal(expected.Trim(), code);
        }

        [Fact]
        public void T09_Should_create_interface_with_optional_field()
        {
            var c = new TsInterface("SampleInterface");

            var f = c.AddField("value");
            f.Type = "number";
            f.Initializer = "7";
            f.IsOptional = true;
            var code = GetCode(c);
            var expected = @"
interface SampleInterface
{
    value?: number;
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T10_Should_create_field_with_inline_comment()
        {
            var f = new TsField("value")
            {
                Type = "number",
                Initializer = "7",
                InlineComment = "converted from int"
            };
            var code = GetCode(f);
            var expected = "value: number = 7; // converted from int";
            Assert.Equal(expected.Trim(), code);
        }

        [Fact]
        public void T11_Should_create_single_line_comment()
        {
            var c = new TsClass("MyClass");
            var f = c.AddField("value", "number");
            f.Introduction = new TsSingleLineComment("some text");

            var code = GetCode(c);
            var expected = @"
class MyClass
{
    // some text
    value: number;
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T12_Should_create_multi_line_comment()
        {
            var c = new TsClass("MyClass");
            var f = c.AddField("value", "number");
            f.Introduction = new TsMultiLineComment("line 1\r\nline 2\nline 3", false);
            var code = GetCode(c);
            var expected = @"
class MyClass
{
    /*
    line 1
    line 2
    line 3
    */
    value: number;
}";
            Assert.Equal(expected.Trim(), code);
            // compact
            f.Introduction = new TsMultiLineComment("line 1\r\nline 2\nline 3");
            code = GetCode(c);
            expected = @"
class MyClass
{
    /* line 1
       line 2
       line 3 */
    value: number;
}";
            Assert.Equal(expected.Trim(), code);
        }


        [Fact]
        public void T13_Should_create_class_with_introduction()
        {
            var c = new TsClass("MyClass");
            var f = c.AddField("value", "number");
            f.Introduction = new TsMultiLineComment("line 1\r\nline 2\nline 3");
            c.Introduction = new TsMultiLineComment("line 4\r\nline 5\nline 6");
            var code = GetCode(c);
            var expected = @"
/* line 4
   line 5
   line 6 */
class MyClass
{
    /* line 1
       line 2
       line 3 */
    value: number;
}";
            Assert.Equal(expected.Trim(), code);
            
        }



        [Fact]
        public void T14_Should_create_enum()
        {
            var c = new TsEnum("MyEnum");
            c.WithItem("None", 0);
            c.WithItem("Left", 2);
            c.WithItem("Right", 17);
            
            c.Introduction = new TsMultiLineComment("Sample enum");
            var code = GetCode(c);
            var expected = @"
/* Sample enum */
enum MyEnum
{
    None = 0
    Left = 2
    Right = 17,
}";
            Assert.Equal(expected.Trim(), code);


            c.IsExported = true;
            code = GetCode(c);
            expected = @"
/* Sample enum */
export enum MyEnum
{
    None = 0
    Left = 2
    Right = 17,
}";
            Assert.Equal(expected.Trim(), code);

        }
    }
}