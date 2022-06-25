using System.Linq.Expressions;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

public static class RhinoMockExtensions
{
    public static IMethodOptions<R> Expect<T, R>(this T mock, Expression<Func<T, R>> func)
        where T : class
    {
        var mockDecorator = mock.GetMockDecorator();
        var setup = mockDecorator.Setup(func);
        return setup.Verifiable();
    }
    
    public static IMethodOptions<object> Expect<T>(this T mock, Expression<Action<T>> action)
        where T : class
    {
        var mockDecorator = mock.GetMockDecorator();
        var setup = mockDecorator.Setup(action);
        return setup.Verifiable();
    }

    public static IMethodOptions<R> Stub<T, R>(this T mock, Expression<Func<T, R>> func)
        where T : class
    {
        var mockDecorator = mock.GetMockDecorator();
        return mockDecorator.Setup(func);
     }

    public static IMethodOptions<object> Stub<T>(this T mock, Expression<Action<T>> action)
        where T : class
    {
        var mockDecorator = mock.GetMockDecorator();
        return mockDecorator.Setup(action);
    }

    public static void VerifyAllExpectations(this object mockObject)
    {
        var mock = mockObject.GetNonTypedMockDecorator().NonTypedMock;
        mock.Verify();
    }

    internal static IMockDecorator<T> GetMockDecorator<T>(this T mockedObject)
        where T : class
    {
        return mockedObject is IMockDecoratorProvider mockDecoratorProvider
            ? mockDecoratorProvider.Get<T>()
            : throw new RhinoMockWrapperException("Not a mock.");
    }

    internal static INonTypedMockDecorator GetNonTypedMockDecorator(this object mockedObject)
    {
        return mockedObject is IMockDecoratorProvider mockDecoratorProvider
            ? mockDecoratorProvider.Get()
            : throw new RhinoMockWrapperException("Not a mock.");
    }
}