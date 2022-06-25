using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Handlers;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Decorators;

internal sealed class FuncSetupDecorator<T, TResult> : IMethodSetupDecorator<T, TResult>
    where T : class
{
    private readonly Mock<T> _mock;
    private readonly Expression<Func<T, TResult>> _expression;
    private readonly ISetup<T, TResult> _setup;

    private Action<ISetup<T, TResult>> _replayActions = _ => { };
    
    public FuncSetupDecorator(Mock<T> mock, Expression<Func<T, TResult>> expression)
    {
        _mock = mock;
        _expression = expression;
        _setup = _mock.Setup(expression);
    }

    public IMethodOptions<TResult> IgnoreArguments()
    {
        IgnoreArgumentsHandler.Handle(_mock, _expression);

        return this;
    }

    public IMethodOptions<TResult> Return(TResult objToReturn)
    {
        _setup.Returns(objToReturn);
        _replayActions += s => s.Returns(objToReturn);

        return this;
    }

    public IMethodOptions<TResult> Throw(Exception exception)
    {
        _setup.Throws(exception);
        _replayActions += s => s.Throws(exception);

        return this;
    }

    public IMethodOptions<TResult> WhenCalled(Action<MethodInvocation> action)
    {
        void InvocationCallback(IInvocation moqInvocation)
        {
            var rhinoInvocation = new MethodInvocation(
                moqInvocation,
                returnValue => _setup.Returns((TResult)returnValue));

            action(rhinoInvocation);
        }

        _setup.Callback(InvocationCallback);
        _replayActions += s => s.Callback(InvocationCallback);

        return this;
    }

    public IMethodSetupDecorator<T, TResult> Verifiable()
    {
        _setup.Verifiable();
        _replayActions += s => s.Verifiable();

        return this;
    }

    public void Replay(Expression? expression = null)
    {
        var setup = _mock.Setup(expression as Expression<Func<T, TResult>> ?? _expression);

        _replayActions(setup);
    }
}