using Moq;

namespace Rhino.Mocks;

internal interface IMockProvider<T> 
    where T : class
{
    Mock<T> Mock { get; }
}