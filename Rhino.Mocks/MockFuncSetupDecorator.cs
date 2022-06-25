using System.Linq.Expressions;
using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

internal sealed class MockFuncSetupDecorator<T, TResult> : IMockSetupDecorator<T, TResult>
    where T : class
{
    private readonly Mock<T> _mock;
    private readonly Expression<Func<T, TResult>> _expression;

    private ISetup<T, TResult> _setup;

    public MockFuncSetupDecorator(Mock<T> mock, Expression<Func<T, TResult>> expression)
    {
        _mock = mock;
        _expression = expression;
        _setup = _mock.Setup(expression);
    }

    public IMethodOptions<TResult> Throw(Exception exception)
    {
        _setup.Throws(exception);
        return this;
    }

    public IMethodOptions<TResult> IgnoreArguments()
    {
        throw new NotImplementedException();
    }

    public IMethodOptions<TResult> Return(TResult objToReturn)
    {
        _setup.Returns(objToReturn);
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

        return this;
    }

    public IMockSetupDecorator<T, TResult> Verifiable()
    {
        _setup.Verifiable();
        return this;
    }

    public void Replay(Expression<Action<T>>? expression = null)
    {
        throw new NotImplementedException();
    }
}