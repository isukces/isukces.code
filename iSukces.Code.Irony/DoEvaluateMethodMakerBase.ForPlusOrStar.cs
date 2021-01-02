using System;
using System.Linq;

namespace iSukces.Code.Irony
{
    public abstract partial class DoEvaluateMethodMakerBase
    {
        public class ForPlusOrStar : DoEvaluateMethodMakerBase
        {
            public ForPlusOrStar(NonTerminalInfo tokenInfo, IDoEvaluateHelper helper, CsClass astClass)
                : base(astClass, tokenInfo) =>
                _helper = helper;

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
                var last = _arg.Kind == ConstructorBuilder.Kinds.BaseConstructor;
                if (_helper.GetDataClassConstructorArgument(_arg, last, body, out var constructorCallExpression))
                {
                    argumentBuilder.Add(constructorCallExpression);
                    return true;
                }

                if (_arg.Kind == ConstructorBuilder.Kinds.BaseConstructor)
                    return false;
                if (MethodsMap.TryGetValue(_arg.Name, out var q))
                {
                    CallOneArgument(q);
                    return true;
                }

                if (!_helper.GetDataClassConstructorArgument(_arg, true, body, out constructorCallExpression))
                    return false;
                argumentBuilder.Add(constructorCallExpression);
                return true;
            }

            private readonly IDoEvaluateHelper _helper;
        }
    }
}