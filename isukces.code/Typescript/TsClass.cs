using System.Collections.Generic;

namespace isukces.code.Typescript
{
    public class TsClass
    {
        public string Name { get; set; }
        public bool IsExported { get; set; }
        public List<TsDecorator> TsDecorators { get; set; } = new List<TsDecorator>();
        public List<TsMethod> Methods { get; set; }=new List<TsMethod>();
    }

    public class TsMethod
    {
        public string Name { get; set; }
        public bool IsStatic { get; set; }
        public string Extends { get; set; }
        public TsVisibility Visibility { get; set; }
        public List<TsMethodArgument> Arguments { get; set; } = new List<TsMethodArgument>();
        public List<TsField> Fields { get; set; } = new List<TsField>();
    }
}
/*
 @Serenity.Decorators.responsive()
    export class MyTasksDialog extends Serenity.EntityDialog<MyTasksRow, any> { 
     
     */