using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

internal sealed class FuncOptions<T,R> : IMethodOptions<R>
    where T : class
{
    private readonly ISetup<T, R> _setup;

    public FuncOptions(ISetup<T,R> setup)
    {
        _setup = setup;
    }

    public IMethodOptions<R> Return(R objToReturn)
    {
        _setup.Returns(objToReturn);
        return this;
    }

    public IMethodOptions<R> WhenCalled(Action<MethodInvocation> action)
    {
        void InvocationCallback(IInvocation moqInvocation)
        {
            var rhinoInvocation = new MethodInvocation(
                moqInvocation,
                returnValue => _setup.Returns((R)returnValue));

            action(rhinoInvocation);
        }

        _setup.Callback(InvocationCallback);

        return this;
    }
}