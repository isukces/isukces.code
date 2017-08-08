using System.Collections.Generic;
using System.Linq;

namespace isukces.code.Typescript
{
    public class TsClass : TsClassOrEnum, ITsCodeProvider
    {
        public TsClass()
        {
        }

        public TsClass(string name)
        {
            Name = name;
        }

        public TsField AddField(string name)
        {
            var f = new TsField(name);
            Members.Add(f);
            return f;
        }

        public TsMethod AddMethod(string name)
        {
            var m = new TsMethod {Name = name};
            Members.Add(m);
            return m;
        }

        public void WriteCodeTo(TsWriteContext cf)
        {
            if (Decorators != null && Decorators.Any())
                foreach (var i in Decorators)
                    i.WriteCodeTo(cf);
            cf.Formatter.Open(string.Join(" ", GetClassHeader()));
            foreach (var i in Members)
                i.WriteCodeTo(cf);
            cf.Formatter.Close();
            /*
             @Serenity.Decorators.registerClass()
    @Serenity.Decorators.responsive()
    export class MyTasksDialog extends Serenity.EntityDialog<MyTasksRow, any> {
             */
        }

        private IEnumerable<string> GetClassHeader()
        {
            if (IsExported)
                yield return "export";
            yield return "class";
            yield return Name;
            if (string.IsNullOrEmpty(Extends)) yield break;
            yield return "extends";
            yield return Extends;
        }

        public List<ITsClassMember> Members { get; set; } = new List<ITsClassMember>();

        public string Extends { get; set; }
    }
}
/*
 @Serenity.Decorators.responsive()
    export class MyTasksDialog extends Serenity.EntityDialog<MyTasksRow, any> { 
     
     */