namespace Rhino.Mocks.Interfaces;

public interface IMethodOptions<T>
{
    IMethodOptions<T> Throw(Exception exception);

    IMethodOptions<T> IgnoreArguments();

    IMethodOptions<T> Return(T objToReturn);

    IMethodOptions<T> WhenCalled(Action<MethodInvocation> action);
}