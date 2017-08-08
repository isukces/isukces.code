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
            var ns = new TsNamespace("Namespace") {IsExport = true};
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
    }
}