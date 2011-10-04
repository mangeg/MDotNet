namespace MDotNet.Extensions
{
	using System.Linq.Expressions;
	using System.Reflection;

	/// <summary>
	/// Type extension methods
	/// </summary>
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Gets expression member info.
		/// </summary>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		public static MemberInfo GetMemberInfo( this Expression expression )
		{
			var lambda = ( LambdaExpression )expression;

			MemberExpression memberExpression;
			if ( lambda.Body is UnaryExpression )
			{
				var unaryExpression = ( UnaryExpression )lambda.Body;
				memberExpression = ( MemberExpression )unaryExpression.Operand;
			}
			else memberExpression = ( MemberExpression )lambda.Body;

			return memberExpression.Member;
		}
	}
}