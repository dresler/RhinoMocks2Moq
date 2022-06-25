using System.Linq.Expressions;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

public static class RhinoMockExtensions
{
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
}