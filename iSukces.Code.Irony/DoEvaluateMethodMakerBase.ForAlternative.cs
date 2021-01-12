using System;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract partial class DoEvaluateMethodMakerBase
    {
        public class ForAlternative : DoEvaluateMethodMakerBase
        {
            public ForAlternative(NonTerminalInfo tokenInfo, IDoEvaluateHelper helper, CsClass astClass) :
                base(astClass, tokenInfo, helper)
            {
            }

            protected override void CreateInternal()
            {
                if (_tokenInfo.CreationInfo.DataConstructor == null) return;

                foreach (var p in _tokenInfo.CreationInfo.DataConstructor.ConstructorArguments)
                {
                    _arg = p;
                    if (AddConstructorArgument())
                        continue;
                    throw new Exception("Unable to get expression for " + p.Name);
                }

                var className = _tokenInfo.CreationInfo.DataClass.Name;
                Finish(className);
                AddDoEvaluateMethod();
            }

            private bool AddConstructorArgument()
            {
                return AddConstructorArgument2(() =>
                {
                    var aaa = _astClass.GetAnnotation<Exchange.Du>(Exchange.X);
                    if (aaa is null)
                        return false;
                    if (_arg.Name == "TmpValue")
                    {
                        //var node = ChildNodes[0]
                        // var v = GetValue(thread, node);
                        body.WriteLine("var altValue = GetValue(thread, ChildNodes[0]);");
                        /// body.WriteLine("var altValue = base.DoEvaluate(thread);");
                        argumentBuilder.Add(new CsExpression("altValue"));
                        return true;
                    }
                    else if (_arg.Name == "NodeKind")
                    {
                        body.WriteLine("var nodeKind = "+aaa.GetNodeKindMethodName+"();");
                        argumentBuilder.Add(new CsExpression("nodeKind"));
                        return true;
                    }

                    return false;
                });
            }
        }
    }
}