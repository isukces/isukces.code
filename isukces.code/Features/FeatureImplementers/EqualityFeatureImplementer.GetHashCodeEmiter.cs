using System.Collections.Generic;
using System.Linq;
using iSukces.Code.AutoCode;
using iSukces.Code.Interfaces;

namespace iSukces.Code.FeatureImplementers;

public partial class EqualityFeatureImplementer
{
    private class GetHashCodeEmiter
    {
        private GetHashCodeEmiter(List<GetHashCodeExpressionDataWithMemberInfo> members, CsCodeWriter cw)
        {
            _members            = members;
            _cw                 = cw;
            _resultVariableName = Find(_members);
        }

        private static CsExpression AppendCode(CsExpression left, GetHashCodeExpressionData right)
        {
            var multiply = right.GetGethashcodeMultiply(DefaultGethashcodeMultiply);
            var result   = left * multiply;
            if (right.HasMinMax)
                result += right.ExpressionWithOffset;
            else
                result ^= right.ExpressionWithOffset;
            return result;
        }

        private static string Find(IReadOnlyCollection<GetHashCodeExpressionDataWithMemberInfo> members)
        {
            const string baseName = "code";
            var          c        = baseName;
            var          nr       = 0;
            while (!Test(c))
            {
                nr++;
                c = baseName + nr;
            }

            return c;

            bool Test(string name)
            {
                return members.Count == 0 || members.All(a => a.Member.Name != name);
            }
        }

        public static void Write(List<GetHashCodeExpressionDataWithMemberInfo> members, CsCodeWriter cw)
        {
            var instance = new GetHashCodeEmiter(members, cw);
            instance.Emit();
        }

        private string CreateFullLine(CsExpression code) =>
            (_variableAlreadyDeclared ? "" : "var ") + $"{_resultVariableName} = {code.Code};";

        private void Emit()
        {
            switch (_members.Count)
            {
                case 0:
                    _cw.WriteLine("return 0;");
                    break;
                case 1:
                    _cw.WriteLine($"return {_members[0].Code.ExpressionWithOffset};");
                    break;
                default:
                    _cw.Open("unchecked");
                    EmitFullCode();
                    _cw.Close();
                    break;
            }
        }

        private void EmitFullCode()
        {
            var lastIdx = _members.Count - 1;
            for (var i = 0; i <= lastIdx; i++)
            {
                var code = _members[i].Code;
                if (_accumulator is null)
                {
                    _accumulator                  = code.ExpressionWithOffset;
                    _propertiesCountInAccumulator = 1;
                }
                else
                {
                    EmitOrAppend(code);
                }
            }

            if (_propertiesCountInAccumulator > 0)
            {
                _cw.WriteLine("return " + _accumulator.Code + ";");
                return;
            }

            _cw.WriteLine($"return {_resultVariableName};");
            return;

            void EmitOrAppend(GetHashCodeExpressionData hc)
            {
                var newCode  = AppendCode(_accumulator, hc);
                var fullLine = CreateFullLine(newCode);
                if (fullLine.Length < MaxExprLength)
                {
                    _accumulator = newCode;
                    _propertiesCountInAccumulator++;
                    return; // try add another expression in next iteration
                }

                if (_propertiesCountInAccumulator == 0)
                {
                    Flush(fullLine); // accumulator was empty so I have to produce longer line than allowed
                    return;
                }

                // emit just accumulator, clear it and add current property to accum
                fullLine = CreateFullLine(_accumulator);
                Flush(fullLine);
                _accumulator                  = AppendCode(_accumulator, hc);
                _propertiesCountInAccumulator = 1;
            }
        }

        private void Flush(string code)
        {
            _cw.WriteLine(code);
            _accumulator                  = new CsExpression(_resultVariableName);
            _variableAlreadyDeclared      = true;
            _propertiesCountInAccumulator = 0;
        }

        private const int MaxExprLength = 120;

        private readonly string _resultVariableName;

        private          CsExpression                                  _accumulator;
        private          int                                           _propertiesCountInAccumulator;
        private          bool                                          _variableAlreadyDeclared;
        private readonly List<GetHashCodeExpressionDataWithMemberInfo> _members;
        private readonly CsCodeWriter                                  _cw;
    }
}
