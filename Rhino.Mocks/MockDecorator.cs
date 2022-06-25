using System.Linq.Expressions;
using Moq;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks;

internal sealed class MockDecorator<T> : IMockDecorator<T>
    where T : class
{
    private readonly Mock<T> _mock;

    public IList<IMockSetupDecorator<T>> Setups { get; } = new List<IMockSetupDecorator<T>>();

    public Mock NonTypedMock => _mock;
    
    public Mock<T> Mock => _mock;

    public IMockSetupDecorator<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
    {
        var setup = new MockFuncSetupDecorator<T,TResult>(Mock, expression);
        Setups.Add(setup);
        return setup;
    }

    public IMockSetupDecorator<T, object> Setup(Expression<Action<T>> expression)
    {
        var setup = new MockActionSetupDecorator<T>(Mock, expression);
        Setups.Add(setup);
        return setup;
    }

    public MockDecorator(Mock<T> mock)
    {
        _mock = mock;
    }
}