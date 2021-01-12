using System;

namespace iSukces.Code.Irony
{
    public abstract partial class DoEvaluateMethodMakerBase
    {
        public class ForPlusOrStar : DoEvaluateMethodMakerBase
        {
            public ForPlusOrStar(NonTerminalInfo tokenInfo, IDoEvaluateHelper helper, CsClass astClass)
                : base(astClass, tokenInfo, helper)
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
                    if (MethodsMap.TryGetValue(_arg.Name, out var q))
                    {
                        CallOneArgument(q);
                        return true;
                    }

                    return false;
                });
            }
        }
    }
}