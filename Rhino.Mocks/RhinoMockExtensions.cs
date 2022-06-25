using System.Linq.Expressions;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

public static class RhinoMockExtensions
{
    public static IMethodOptions<R> Expect<T, R>(this T mock, Expression<Func<T, R>> func)
        where T : class
    {
        if (mock is IMockProvider<T> mockProvider)
        {
            var setup = mockProvider.Mock.Setup(func);
            setup.Verifiable();

            return new FuncOptions<T, R>(setup);
        }

        throw new RhinoMockWrapperException("Not a mock.");
    }
    
    public static IMethodOptions<object> Expect<T>(this T mock, Expression<Action<T>> action)
        where T : class
    {
        if (mock is IMockProvider<T> mockProvider)
        {
            var setup = mockProvider.Mock.Setup(action);
            setup.Verifiable();

            return new ActionOptions<T>(setup);
        }

        throw new RhinoMockWrapperException("Not a mock.");
    }

    public static IMethodOptions<R> Stub<T, R>(this T mock, Expression<Func<T, R>> func)
        where T : class
    {
        if (mock is IMockProvider<T> mockProvider)
        {
            var setup = mockProvider.Mock.Setup(func);
            return new FuncOptions<T,R>(setup);
        }

        throw new RhinoMockWrapperException("Not a mock.");
    }

    public static IMethodOptions<object> Stub<T>(this T mock, Expression<Action<T>> action)
        where T : class
    {
        if (mock is IMockProvider<T> mockProvider)
        {
            var setup = mockProvider.Mock.Setup(action);
            return new ActionOptions<T>(setup);
        }

        throw new RhinoMockWrapperException("Not a mock.");
    }

    public static void VerifyAllExpectations(this object mockObject)
    {
        if (mockObject is INonTypedMockProvider mockProvider)
        {
            var mock = mockProvider.Mock;
            mock.Verify();
            return;
        }

        throw new RhinoMockWrapperException("Not a mock.");
    }
}