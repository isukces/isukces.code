#nullable disable
using iSukces.Code.Interfaces;
// ReSharper disable All

namespace iSukces.Code.Tests.EqualityGenerator
{
    public partial class EqualityGeneratorTests
    {

        public enum EnumWithOffset
        {
            One = 5,
            Two = 8,
            Three = 12
        }

        public enum NormalEnum
        {
            One,
            Two,
            Three
        }


        [Auto.EqualityGeneratorAttribute]
        public partial struct OnePropertyStruct
        {
            public int IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class OnePropertyClass
        {
            public int IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties1
        {
            public NormalEnum     N1 { get; set; }
            public EnumWithOffset O1 { get; set; }
            public NormalEnum     N2 { get; set; }
            public EnumWithOffset O2 { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties2
        {
            public int        IntValue { get; set; }
            public NormalEnum Normal   { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties3
        {
            public int            IntValue { get; set; }
            public EnumWithOffset Offset   { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithEnumProperties4
        {
            public EnumWithOffset Offset   { get; set; }
            public int            IntValue { get; set; }
        }

        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithManyProperties
        {
            public EnumWithOffset Offset       { get; set; }
            public int            IntValue     { get; set; }
            public uint           UIntValue    { get; set; }
            public short          ShortValue   { get; set; }
            public ushort         UShortValue  { get; set; }
            public decimal        DecimalValue { get; set; }

            public string NameNotNull { get; set; }

            [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
            public string Id { get; set; }

            [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
            public string IdNotNull { get; set; }
        }
        
        
        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithNullable
        {
            public EnumWithOffset? EnumNullable { get; set; }
            public int?            IntNullable  { get; set; }
            
            public  int GetHashCode2()
            {
                unchecked
                {
                    return (IntNullable ?? 0) * 8 + ((int?)EnumNullable ?? 5) - 5;
                }
            }
        }


        [Auto.EqualityGeneratorAttribute]
        public partial class ClassWithBools
        {
            public int            Value1       { get; set; }
            public EnumWithOffset EnumNullable { get; set; }
            public int            Value2       { get; set; }
            public bool           BoolNormal   { get; set; }
            public int            Value3       { get; set; }
            public bool?          BoolNullable { get; set; }
            public int            Value4       { get; set; }
        }

    }
}
