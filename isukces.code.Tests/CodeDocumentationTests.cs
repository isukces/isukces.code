using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace iSukces.Code.Tests;

public class CodeDocumentationTests
{
    public static IEnumerable<object[]> GetTypes()
    {
        return CodeDocumentationTestsSource.GetNumbers().Select(a => a.ToObjectArray()).ToList();
    }

    [Theory]
    [MemberData(nameof(GetTypes))]
    public void ConvertTypeToDocFormat(Type source, string expected)
    {
        var actual = MethodInfoConverter.TypeToString(source);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    ///     Definition of Sample1 method
    /// </summary>
    /// <param name="xint">arg 1</param>
    /// <param name="xuint">arg 1</param>
    /// <param name="xbyte"></param>
    /// <param name="xsbyte"></param>
    /// <param name="xshort"></param>
    /// <param name="xushort"></param>
    /// <param name="xlong"></param>
    /// <param name="xulong"></param>
    /// <param name="xfloat"></param>
    /// <param name="xdouble"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int Sample1(
        int xint, uint xuint,
        byte xbyte, sbyte xsbyte,
        short xshort, ushort xushort,
        long xlong, ulong xulong,
        float xfloat, double xdouble
    )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Definition of Sample2 method
    /// </summary>
    /// <param name="list">arg 1</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int Sample2(List<string> list)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Definition of Sample3 method
    /// </summary>
    /// <param name="dictionary">arg 1</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<int> Sample3(DictionaryList<int, string> dictionary)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Definition of Sample4 method
    /// </summary>
    /// <param name="dictionary">arg 1</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<IEnumerable<double[]>> Sample4(List<IEnumerable<double[]>> dictionary)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Definition of Sample5 method
    /// </summary>
    /// <param name="dictionary">arg 1</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<IEnumerable<double[]>> Sample5(out List<IEnumerable<float[]>> dictionary)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Definition of Sample6 method
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<IEnumerable<double[]>> Sample6()
    {
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData(nameof(Sample1),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample1(System.Int32,System.UInt32,System.Byte,System.SByte,System.Int16,System.UInt16,System.Int64,System.UInt64,System.Single,System.Double)")]
    [InlineData(nameof(Sample2),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample2(System.Collections.Generic.List{System.String})")]
    [InlineData(nameof(Sample3),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample3(iSukces.Code.DictionaryList{System.Int32,System.String})")]
    [InlineData(nameof(Sample4),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample4(System.Collections.Generic.List{System.Collections.Generic.IEnumerable{System.Double[]}})")]
    [InlineData(nameof(Sample5),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample5(System.Collections.Generic.List{System.Collections.Generic.IEnumerable{System.Single[]}}@)")]
    [InlineData(nameof(Sample6),
        "iSukces.Code.Tests.CodeDocumentationTests.Sample6")]
    public void T01_Should_Get_sample1_method_key(string methodName, string expected)
    {
        var flags = BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.Static;
        var method = typeof(CodeDocumentationTests).GetMethod(methodName, flags);
        var key    = MethodInfoConverter.GetKey(method);
        Assert.Equal(expected, key?.Name);
    }
}

public readonly record struct CodeDocumentationTestsSource(Type Type, string Expected)
{
    public static IEnumerable<CodeDocumentationTestsSource> GetNumbers()
    {
        yield return new CodeDocumentationTestsSource(
            typeof(int), "System.Int32");
        yield return new CodeDocumentationTestsSource(
            typeof(List<string>),
            "System.Collections.Generic.List{System.String}");
        yield return new CodeDocumentationTestsSource(
            typeof(List<IEnumerable<float[]>>),
            "System.Collections.Generic.List{System.Collections.Generic.IEnumerable{System.Single[]}}");
    }

    public object[] ToObjectArray()
    {
        return new object[] { Type, Expected };
    }
}