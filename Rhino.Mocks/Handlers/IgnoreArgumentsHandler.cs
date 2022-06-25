using System.Linq.Expressions;
using Moq;
using Rhino.Mocks.Decorators;

namespace Rhino.Mocks.Handlers;

internal static class IgnoreArgumentsHandler
{
    public static void Handle<T, TDelegate>(Mock<T> mock, Expression<TDelegate>? expression)
        where T : class
    {
        // Replace all setup calls by a call with replacing it with an always matching setup
        var expressionIgnoringArguments = new MakeAnyVisitor().VisitAndConvert(expression, nameof(IgnoreArgumentsHandler));

        var mockDecorator = mock.Object.GetMockDecorator();
        var setupsToReplay = mockDecorator.Setups.ToArray();

        mock.Reset();

        mockDecorator.Setups.Clear();

        MockDecoratorHelper.Set(mock, mockDecorator);
        
        foreach (var setup in setupsToReplay)
        {
            var setupExpression = setup == setupsToReplay.Last()
                ? expressionIgnoringArguments
                : null;

            setup.Replay(setupExpression);
        }
    }

    private class MakeAnyVisitor : ExpressionVisitor
    {
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.Value != null)
                return base.VisitConstant(node);

            var method = typeof(It)
                .GetMethod("IsAny")!
                .MakeGenericMethod(node.Type);

            return Expression.Call(method);
        }
    }

}