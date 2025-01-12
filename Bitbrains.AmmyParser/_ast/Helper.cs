#nullable disable
using System;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    internal static class Helper
    {
        public static Tuple<T> Get<T>(this ParseTreeNodeList nodes)
        {
            if (nodes.Count != 1)
                return null;
            if (nodes[0]?.AstNode is T x)
                return new Tuple<T>(x);
            return null;
        }

        public static Tuple<T, T2> Get<T, T2>(this ParseTreeNodeList nodes)
        {
            if (nodes.Count != 1)
                return null;
            if (nodes[0]?.AstNode is T x)
                if (nodes[1]?.AstNode is T2 y)
                    return new Tuple<T, T2>(x, y);
            return null;
        }
    }
}
