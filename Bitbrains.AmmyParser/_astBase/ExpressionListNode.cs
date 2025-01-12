#nullable disable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public class ExpressionListNode<T> : AstNode
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
            foreach (var childNode in treeNode.ChildNodes)
                AddChild(NodeUseType.Parameter, "expr", childNode);
            AsString = "Expression list of type " + typeof(T);
        }

        protected override object DoEvaluate(ScriptThread thread)
        {
            thread.CurrentNode = this;
            var items = new T[ChildNodes.Count];
            for (var index = 0; index < items.Length; ++index)
            {
                var childNode = ChildNodes[index];
                var value = childNode.Evaluate(thread);
                if (value is T t)
                    items[index] = t;
                else if (value is null && (typeof(T).IsInterface || typeof(T).IsClass))
                {
                    Debug.Write("");
                }
                else
                {
                    var src = value is null ? "NULL" : value.GetType().ToString();
                    var msg = $"Invalid cast {src} to {typeof(T)}, owner {GetType()}";
                    throw new Exception(msg);
                }
            }

            thread.CurrentNode = Parent;
            return MakeCollection(items);
        }

        protected virtual object MakeCollection(IReadOnlyList<T> items)
        {
            return items;
        }
    }
}
