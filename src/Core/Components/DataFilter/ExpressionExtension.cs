using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Microsoft.FluentUI.AspNetCore.Components;

internal static class ExpressionExtension
{
    public static Expression<Func<T, bool>> Make<T>(this Expression firstExpression, Expression secondExpression)
    {
        var bodyIdentifier = new ExpressionBodyIdentifier();

        var body = bodyIdentifier.Identify(firstExpression);

        var parameterIdentifier = new ExpressionParameterIdentifier();
        var parameter = (ParameterExpression)parameterIdentifier.Identify(firstExpression);

        var body2 = bodyIdentifier.Identify(secondExpression);
        var parameter2 = (ParameterExpression)parameterIdentifier.Identify(secondExpression);
        var treeModifier = new ExpressionReplacer(parameter2, body);

        return Expression.Lambda<Func<T, bool>>(treeModifier.Visit(body2), parameter);
    }

    public static Expression<Func<T, bool>> Make<T>(this Expression expression, ExpressionType binaryOperation, object? value)
    {
        var bodyIdentifier = new ExpressionBodyIdentifier();
        var body = bodyIdentifier.Identify(expression);
        var parameterIdentifier = new ExpressionParameterIdentifier();
        var parameter = (ParameterExpression)parameterIdentifier.Identify(expression);
        BinaryExpression? binaryExpression;

        if (Nullable.GetUnderlyingType(body.Type) is not null)
        {
            binaryExpression = Expression.MakeBinary(binaryOperation, body, Expression.Convert(Expression.Constant(value), body.Type));
        }
        else
        {
            if (value is null)
            {
                return x => true;
            }

            binaryExpression = Expression.MakeBinary(binaryOperation, body, Expression.Convert(Expression.Constant(value), body.Type));
        }

        return Expression.Lambda<Func<T, bool>>(binaryExpression, parameter);
    }

    public class ExpressionReplacer(Expression from, Expression to) : ExpressionVisitor
    {
        private readonly Expression _from = from;
        private readonly Expression _to = to;

        [return: NotNullIfNotNull(nameof(node))]
        public override Expression? Visit(Expression? node)
            => node == _from
                ? _to
                : base.Visit(node);
    }

    public class ExpressionBodyIdentifier : ExpressionVisitor
    {
        public Expression Identify(Expression node) => base.Visit(node);

        protected override Expression VisitLambda<T>(Expression<T> node) => node.Body;
    }

    public class ExpressionParameterIdentifier : ExpressionVisitor
    {
        public Expression Identify(Expression node) => base.Visit(node);

        protected override Expression VisitLambda<T>(Expression<T> node) => node.Parameters[0];
    }
}
