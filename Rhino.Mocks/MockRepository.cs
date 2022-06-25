using Moq;
using Rhino.Mocks.Decorators;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks;

public class MockRepository
{
    public static T GenerateMock<T>(params object[] argumentsForConstructor)
        where T : class
    {
        if (argumentsForConstructor.Any())
            throw new RhinoMockWrapperException($"{nameof(argumentsForConstructor)} are not supported.");

        var mock = new Mock<T>();

        MockDecoratorHelper.Set(mock);
        
        return mock.Object;
    }
}