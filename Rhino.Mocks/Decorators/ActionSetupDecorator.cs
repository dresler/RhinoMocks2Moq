using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Exceptions;
using Rhino.Mocks.Handlers;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Decorators;

internal sealed class ActionSetupDecorator<T> : IMethodSetupDecorator<T, object>
    where T : class
{
    private readonly Mock<T> _mock;
    private readonly Expression<Action<T>> _expression;
    private readonly ISetup<T> _setup;

    private Action<ISetup<T>> _replayActions = _ => { };
    
    public ActionSetupDecorator(Mock<T> mock, Expression<Action<T>> expression)
    {
        _mock = mock;
        _expression = expression;
        _setup = _mock.Setup(expression);
    }

    public IMethodOptions<object> IgnoreArguments()
    {
        IgnoreArgumentsHandler.Handle(_mock, _expression);

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

    public IMethodSetupDecorator<T, object> Verifiable()
    {
        _setup.Verifiable();
        _replayActions += s => s.Verifiable();

        return this;
    }

    public void Replay(Expression? expression = null)
    {
        var setup = _mock.Setup(expression as Expression<Action<T>> ?? _expression);
        _replayActions(setup);
    }
}