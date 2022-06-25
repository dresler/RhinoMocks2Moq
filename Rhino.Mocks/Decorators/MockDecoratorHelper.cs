using Moq;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Decorators;

internal static class MockDecoratorHelper
{
    public static void Set<T>(Mock<T> mock, IMockDecorator<T>? mockDecorator = null)
        where T : class
    {
        mockDecorator ??= new MockDecorator<T>(mock);

        var mockDecoratorProvider = mock.As<IMockDecoratorProvider>();
        mockDecoratorProvider.Setup(x => x.Get()).Returns(mockDecorator);
        mockDecoratorProvider.Setup(x => x.Get<T>()).Returns(mockDecorator);
    }
}