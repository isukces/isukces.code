using System;
using Irony.Ast;
using Irony.Interpreter;
using Irony.Interpreter.Ast;
using Irony.Parsing;

namespace Bitbrains.AmmyParser
{
    public partial class AstAmmyBindSetBindingGroupName
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    internal class AstAmmyBindSetBindsDirectlyToSource : AstNode, IAstAmmyBindSetItemProvider
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread) => throw new NotImplementedException();

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetFallbackValue
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetMode
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetIsAsync
    {
        public override void Init(AstContext context, ParseTreeNode treeNode)
        {
            base.Init(context, treeNode);
        }

        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetNotifyOnSourceUpdated
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetNotifyOnTargetUpdated
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }


    partial class AstAmmyBindSetNotifyOnValidationError
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetStringFormat
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    internal class AstAmmyBindSetUpdateSourceTrigger : AstNode, IAstAmmyBindSetItemProvider
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread) => throw new NotImplementedException();

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetValidatesOnDataErrors
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetValidatesOnExceptions
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetValidatesOnNotifyDataErrors
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var data = base.DoEvaluate(thread);
            return new AstAmmyBindSetItemData(Keyword, data);
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }

    partial class AstAmmyBindSetItem : IAstAmmyBindSetItemProvider
    {
        public IAstAmmyBindSetItem GetData(ScriptThread thread)
        {
            var x = base.DoEvaluate(thread);
            return (IAstAmmyBindSetItem)x;
        }

        protected override object DoEvaluate(ScriptThread thread) => GetData(thread);
    }


    public class AstAmmyBindSetItemData : IAstAmmyBindSetItem
    {
        public override string ToString() => PropertyName + ": " + Value;

        public AstAmmyBindSetItemData(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value        = value;
        }

        public string PropertyName { get; }
        public object Value        { get; }
    }
}