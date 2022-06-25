using System.Linq.Expressions;
using Moq;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks.Decorators;

internal sealed class MockDecorator<T> : IMockDecorator<T>
    where T : class
{
    private readonly Mock<T> _mock;

    public IList<IMethodSetupDecorator<T>> Setups { get; } = new List<IMethodSetupDecorator<T>>();

    public Mock NonTypedMock => _mock;
    
    public Mock<T> Mock => _mock;

    public IMethodSetupDecorator<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression)
    {
        var setup = new FuncSetupDecorator<T,TResult>(Mock, expression);
        Setups.Add(setup);

        return setup;
    }

    public IMethodSetupDecorator<T, object> Setup(Expression<Action<T>> expression)
    {
        var setup = new ActionSetupDecorator<T>(Mock, expression);
        Setups.Add(setup);

        return setup;
    }

    public MockDecorator(Mock<T> mock)
    {
        _mock = mock;
    }
}