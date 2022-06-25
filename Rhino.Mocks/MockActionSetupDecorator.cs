using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

internal sealed class MockActionSetupDecorator<T> : IMockSetupDecorator<T, object>
    where T : class
{
    private readonly Mock<T> _mock;
    private readonly Expression<Action<T>> _expression;

    private Action<ISetup<T>> _replayActions = _ => { };

    private readonly ISetup<T> _setup;

    public MockActionSetupDecorator(Mock<T> mock, Expression<Action<T>> expression)
    {
        _mock = mock;
        _expression = expression;
        _setup = _mock.Setup(expression);
    }

    public IMethodOptions<object> IgnoreArguments()
    {
        // Replace all setup calls by a call with replacing it with an always matching setup
        var actionIgnoringArguments = new MakeAnyVisitor().VisitAndConvert(_expression, nameof(IgnoreArguments));
        
        var mockDecorator = _mock.Object.GetMockDecorator();
        var setupsToReplay = mockDecorator.Setups.ToArray();

        _mock.Reset();
        
        mockDecorator.Setups.Clear();

        var mockDecoratorProvider = _mock.As<IMockDecoratorProvider>();
        mockDecoratorProvider.Setup(x => x.Get()).Returns(mockDecorator);
        mockDecoratorProvider.Setup(x => x.Get<T>()).Returns(mockDecorator);

        foreach (var setup in setupsToReplay)
        {
            var expression = setup == setupsToReplay.Last()
                ? actionIgnoringArguments
                : null;

            setup.Replay(expression);
        }

        return this;
    }

    public IMethodOptions<object> Return(object objToReturn)
    {
        throw new RhinoMockWrapperException("Stubbing return value for Action is not supported.");
    }

    public IMethodOptions<object> Throw(Exception exception)
    {
        _setup.Throws(exception);
        _replayActions += s => s.Throws(exception);

        return this;
    }

    public IMethodOptions<object> WhenCalled(Action<MethodInvocation> action)
    {
        void InvocationCallback(IInvocation moqInvocation)
        {
            var rhinoInvocation = new MethodInvocation(
                moqInvocation,
                _ => throw new RhinoMockWrapperException("Setting return value for Action is not supported."));

            action(rhinoInvocation);
        }

        _setup.Callback(InvocationCallback);
        _replayActions += s => s.Callback(InvocationCallback);

        return this;
    }

    public IMockSetupDecorator<T, object> Verifiable()
    {
        _setup.Verifiable();
        _replayActions += s => s.Verifiable();

        return this;
    }

    public void Replay(Expression<Action<T>>? expression = null)
    {
        var setup = _mock.Setup(expression ?? _expression);
        _replayActions(setup);
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