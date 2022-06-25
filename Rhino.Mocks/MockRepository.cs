using Moq;

namespace Rhino.Mocks;

public class MockRepository
{
    public static T GenerateMock<T>(params object[] argumentsForConstructor)
        where T : class
    {
        if (argumentsForConstructor.Any())
            throw new RhinoMockWrapperException($"{nameof(argumentsForConstructor)} are not supported.");

        var mock = new Mock<T>();

        var nonTypedMockProvider = mock.As<INonTypedMockProvider>();
        nonTypedMockProvider.Setup(x => x.Mock).Returns(mock);

        var mockProvider = mock.As<IMockProvider<T>>();
        mockProvider.Setup(x => x.Mock).Returns(mock);

        return mock.Object;
    }
}