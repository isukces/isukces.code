#nullable disable
using System;
using System.Runtime.CompilerServices;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Tests.EqualityGenerator
{
    /// <summary>
    ///     Identyfikuje unikalne połączenie (dwóch jeśli instalacja jest poprawna) drutów
    /// </summary>
    [Auto.EqualityGeneratorAttribute(nameof(IsEmpty), GetHashCodeProperties = new[] {nameof(HashCode)})]
    public partial struct TestStructWithSpecialHashCodeField
    {
        public TestStructWithSpecialHashCodeField(string name)
        {
            Name      = name?.Trim();
            _hasValue = !string.IsNullOrEmpty(Name);
            HashCode  = _hasValue ? StringComparer.Ordinal.GetHashCode(Name) : 0;
        }


        [Auto.EqualityGeneratorSkipAttribute]
        public bool IsGround
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Name == "GROUND"; }
        }

        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return !_hasValue; }
        }

        [Auto.StringComparison.OrdinalIgnoreCaseAttribute]
        public string Name { get; }

        [Auto.EqualityGeneratorSkipAttribute]
        private int HashCode { get; }


        private readonly bool _hasValue;
 
    }
}
