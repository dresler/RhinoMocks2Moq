namespace Rhino.Mocks.Tests;

internal sealed class AnyValueFactory
{
    public static string CreateAnyString() => Guid.NewGuid().ToString();
}