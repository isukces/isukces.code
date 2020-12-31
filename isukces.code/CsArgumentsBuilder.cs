using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace iSukces.Code
{
    public sealed class CsArgumentsBuilder
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CsArgumentsBuilder AddCode(string ammyCode)
        {
            Items.Add(ammyCode);
            return this;
        }

        public CsArgumentsBuilder AddValue(string text) => AddCode(text.CsEncode());

        public override string ToString() => Code;

        public string Code
        {
            get { return string.Join(", ", Items); }
        }

        public string CodeEx
        {
            get { return "(" + Code + ")"; }
        }

        public List<string> Items { get; } = new List<string>();
    }
}