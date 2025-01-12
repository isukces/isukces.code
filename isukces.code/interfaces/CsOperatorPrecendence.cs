using System.Runtime.CompilerServices;

namespace iSukces.Code.Interfaces
{
	/// <summary>
	/// See https://docs.microsoft.com/en-us/cpp/c-language/precedence-and-order-of-evaluation?view=vs-2019
	/// </summary>
	public enum CsOperatorPrecendence
	{
		//[ ] ( ) . -> ++ -- (postfix)
		//Left to right
		Expression,


		//sizeof & * + - ~ ! ++ -- (prefix)
		//Right to left
		Unary,


		//typecasts
		//Right to left
		UnaryTypecast,

		//* / %
		//Left to right
		Multiplicative,

		//+ -
		// Left to right
		Additive,

		//<< >>
		//Left to right
		BitwiseShift,

		//< > <= >=
		//Left to right
		Relational,

		//== !=
		//Left to right
		Equality,

		//&
		//Left to right
		//BitwiseAnd

		//^
		//Left to right
		BitwiseExOr,

		//|
		// Left to right
		BitwiseOr,

		//&&
		//Left to right
		LogicalAnd,

		//||
		//Left to right
		LogicalOr,

		//? :
		//Right to left
		ConditionalExpression,

		// = *= /= %=
		// += -= <<= >>= &=
		// ^= |=
		//Right to left
		SimpleAndCompoundAssignment,

		//,
		//Left to right
		SequentialEvaluation
	}

	public enum CsOperatorPrecendenceAlign
	{
		/// <summary>
		/// Like addition: 1+2+3 is equvalent to (1+2)+3
		/// </summary>
		LeftToRight,

		/// <summary>
		/// Like = : a=b=c is equvalent to b=c and then a=b
		/// </summary>
		RightToLeft
	}

	public enum ExpressionAppend
	{
		After,
		Before
	}

	public static class CsOperatorPrecendenceUtils
	{
		public const CsOperatorPrecendence DoubleQuestion = CsOperatorPrecendence.ConditionalExpression;
		
		public static CsOperatorPrecendenceAlign GetAlignment(this CsOperatorPrecendence codePrecedence)
		{
			var isRightToLeft = codePrecedence == CsOperatorPrecendence.Unary
			                    || codePrecedence == CsOperatorPrecendence.UnaryTypecast
			                    || codePrecedence == CsOperatorPrecendence.ConditionalExpression
			                    || codePrecedence == CsOperatorPrecendence.SimpleAndCompoundAssignment;
			return isRightToLeft ? CsOperatorPrecendenceAlign.RightToLeft : CsOperatorPrecendenceAlign.LeftToRight;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool AddBrackets(CsOperatorPrecendence codePrecedence,
			CsOperatorPrecendence outerExpression, ExpressionAppend append)
		{
			if (outerExpression > codePrecedence) return false;
			if (outerExpression == codePrecedence)
			{
				var al   = GetAlignment(outerExpression);
				var skip = al == CsOperatorPrecendenceAlign.LeftToRight ^ append == ExpressionAppend.After;
				if (skip)
					return false;
			}

			return true;
		}
	}
}
