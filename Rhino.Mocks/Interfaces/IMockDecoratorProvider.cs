namespace Rhino.Mocks.Interfaces;

internal interface IMockDecoratorProvider
{
    INonTypedMockDecorator Get();

    IMockDecorator<T> Get<T>() where T : class;
}