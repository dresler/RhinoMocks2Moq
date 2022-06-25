using Moq;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

public class MockRepository
{
    public static T GenerateMock<T>(params object[] argumentsForConstructor)
        where T : class
    {
        if (argumentsForConstructor.Any())
            throw new RhinoMockWrapperException($"{nameof(argumentsForConstructor)} are not supported.");

        var mock = new Mock<T>();
        var mockDecorator = new MockDecorator<T>(mock);
        
        var mockDecoratorProvider = mock.As<IMockDecoratorProvider>();
        mockDecoratorProvider.Setup(x => x.Get()).Returns(mockDecorator);
        mockDecoratorProvider.Setup(x => x.Get<T>()).Returns(mockDecorator);

        return mock.Object;
    }
}