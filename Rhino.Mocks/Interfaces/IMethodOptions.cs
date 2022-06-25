namespace Rhino.Mocks.Interfaces;

public interface IMethodOptions<T>
{
    IMethodOptions<T> Return(T objToReturn);

    IMethodOptions<T> WhenCalled(Action<MethodInvocation> action);
}