using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
{
    public abstract class TsClassOrInterface : TsNamespaceMember
    {
        protected TsClassOrInterface(bool isInterface, string name)
        {
            IsInterface = isInterface;
            Name = name;
        }

        public TsField AddField(string name, string type = null)
        {
            var f = new TsField(name) {Type = type};
            Members.Add(f);
            return f;
        }

        public TsMethod AddMethod(string name)
        {
            var m = new TsMethod {Name = name};
            Members.Add(m);
            return m;
        }


        public override void WriteCodeTo(TsWriteContext context)
        {
            if (IsInterface)
                context = context.CopyWithFlag(TsWriteContextFlags.HeadersOnly);
            Introduction?.WriteCodeTo(context);
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(context);
            context.Formatter.Open(string.Join(" ", GetClassHeader()));
            foreach (var i in Members)
                i.WriteCodeTo(context);
            context.Formatter.Close();
            /*
             @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    export class MyTasksDialog extends Serenity.EntityDialog<MyTasksRow, any> {
             */
        }


        protected abstract IEnumerable<string> GetClassHeader();

        public List<ITsCodeProvider> Members { get; set; } = new List<ITsCodeProvider>();
        protected bool IsInterface;
    }
}