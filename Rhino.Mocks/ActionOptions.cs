using Moq;
using Moq.Language.Flow;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

internal sealed class ActionOptions<T> : IMethodOptions<object>
    where T : class
{
    private readonly ISetup<T> _setup;

    public ActionOptions(ISetup<T> setup)
    {
        _setup = setup ?? throw new ArgumentNullException(nameof(setup));
    }

    public IMethodOptions<object> Return(object objToReturn)
    {
        throw new RhinoMockWrapperException("Stubbing return value for Action is not supported.");
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

        return this;
    }
}