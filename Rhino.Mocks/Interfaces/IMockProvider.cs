using Moq;

namespace Rhino.Mocks.Interfaces;

internal interface INonTypedMockProvider
{
    Mock Mock { get; }
}

internal interface IMockProvider<T>
    where T : class
{
    Mock<T> Mock { get; }
}