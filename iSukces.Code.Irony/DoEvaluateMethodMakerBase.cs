using System;
using System.Collections.Generic;
using Irony.Interpreter;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.Irony
{
    public abstract partial class DoEvaluateMethodMakerBase
    {
        private bool AddConstructorArgument2(Func<bool> ac)
        {
            var last = _arg.Kind == ConstructorBuilder.Kinds.BaseConstructor;
            var inp  = new Input34(_arg, last, body);
            if (_helper.GetDataClassConstructorArgument(inp))
            {
                argumentBuilder.Add(inp.Expression);
                return true;
            }

            if (_arg.Kind == ConstructorBuilder.Kinds.BaseConstructor)
                return false;
            if (ac())
                return true;
 

            inp = new Input34(_arg, true, body);
            if (!_helper.GetDataClassConstructorArgument(inp))
                return false;
            argumentBuilder.Add(inp.Expression);
            return true;
        }
        
        protected DoEvaluateMethodMakerBase(CsClass astClass, NonTerminalInfo tokenInfo,IDoEvaluateHelper helper)
        {
            _astClass    = astClass;
            _tokenInfo   = tokenInfo;
            _helper = helper;
        }

        public void Create()
        {
            if (_tokenInfo.CreationInfo.DataConstructor == null) return;
            body            = CsCodeWriter.Create(GetType());
            argumentBuilder = new CsArgumentsBuilder();
            CreateInternal();
        }

        protected void AddDoEvaluateMethod()
        {
            /*
 * protected override object DoEvaluate(ScriptThread thread)
{
var fullTypeName   = GetFullTypeNameValue();
var baseObjectType = GetBaseObjectTypeValue(thread);
var body           = GetBodyValue(thread);
var r              = new AmmyMainObjectStatement(Span, baseObjectType, fullTypeName, body);
return r;
}
 * 
 */
            var m = _astClass.AddMethod("DoEvaluate", CsType.Object)
                .WithOverride()
                .WithVisibility(Visibilities.Protected)
                .WithBody(body);

            m.AddParam<ScriptThread>(thread, _astClass);
        }

        protected abstract void CreateInternal();

        private void CallOneArgument(Info info)
        {
            var varName = _arg.Name.FirstLower();
            var aa      = new CsArgumentsBuilder();
            if (info.NeedThread)
                aa.AddCode("thread");
            var code = $"var {varName} = {aa.CallMethod(info.Method, true)}";
            body.WriteLine(code);
            argumentBuilder.Add(new CsExpression(varName));
        }

        private void Finish(CsType className)
        {
            const string varName = "doEvaluateResult";
            body.WriteLine("var " + varName + " = " + argumentBuilder.CallMethod("new " + className, true));
            body.WriteLine("return " + varName + ";");
        }

        protected IReadOnlyDictionary<string, Info> MethodsMap
        {
            get
            {
                if (_methodsMap != null) return _methodsMap;
                _methodsMap = new Dictionary<string, Info>();
                foreach (var m in _astClass.Methods)
                {
                    var q = m.GetAnnotation<Exchange.MethodForEvaluatingProperty>(Exchange.MethodForEvaluatingPropertyKey);
                    if (q is null)
                        continue;
                    var mapPropertyName = q.PropertyName;
                    if (!string.IsNullOrEmpty(mapPropertyName))
                        _methodsMap.Add(mapPropertyName, new Info(m.Name, q.NeedThread));
                }

                return _methodsMap;

            }
        }


        private Dictionary<string, Info> _methodsMap;

        protected CsCodeWriter body;
        protected CsArgumentsBuilder argumentBuilder;
        protected ConstructorBuilder.Argument _arg;

        protected readonly CsClass _astClass;
        protected readonly NonTerminalInfo _tokenInfo;
        protected readonly IDoEvaluateHelper _helper;

        public struct Info
        {
            public Info(string method, bool needThread)
            {
                Method     = method;
                NeedThread = needThread;
            }

            public string Method     { get; }
            public bool   NeedThread { get; }
        }

        protected const string thread = "thread";
    }
}