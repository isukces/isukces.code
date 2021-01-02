using System;
using System.Collections.Generic;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract partial class DoEvaluateMethodMakerBase
    {
        public class ForSequenceRule : DoEvaluateMethodMakerBase
        {
            public ForSequenceRule(NonTerminalInfo tokenInfo, IDoEvaluateHelper helper, CsClass astClass)
                : base(astClass, tokenInfo) =>
                _helper = helper;

            protected override void CreateInternal()
            {


                foreach (var p in _tokenInfo.CreationInfo.DataConstructor.ConstructorArguments)
                {
                    this._arg = p;
                    
                    if (AddConstructorArgument(p))
                        continue;
                    throw new Exception("Unable to get expression for " + p.Name);
                }

                var          className = _tokenInfo.CreationInfo.DataClass.Name;
                Finish(className);
                AddDoEvaluateMethod();
            }

            private bool AddConstructorArgument(ConstructorBuilder.Argument arg)
            {
                var last = arg.Kind == ConstructorBuilder.Kinds.BaseConstructor;
                if (_helper.GetDataClassConstructorArgument(arg, last, body, out var constructorCallExpression))
                {
                    argumentBuilder.Add(constructorCallExpression);
                    return true;
                }

                if (arg.Kind == ConstructorBuilder.Kinds.BaseConstructor)
                    return false;

                if (MethodsMap.TryGetValue(arg.Name, out var info))
                {
                    CallOneArgument(info);
                    return true;
                }

                if (!_helper.GetDataClassConstructorArgument(arg, true, body, out constructorCallExpression))
                    return false;
                argumentBuilder.Add(constructorCallExpression);
                return true;
            }

           

            private readonly IDoEvaluateHelper _helper;
   
        }
    }
}