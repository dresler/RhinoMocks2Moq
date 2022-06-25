using System.Linq.Expressions;
using Moq;

namespace Rhino.Mocks.Interfaces;

internal interface INonTypedMockDecorator
{
    Mock NonTypedMock { get; }
}

internal interface IMockDecorator<T> : INonTypedMockDecorator
    where T : class
{
    Mock<T> Mock { get; }

    IMockSetupDecorator<T, TResult> Setup<TResult>(Expression<Func<T, TResult>> expression);

    IMockSetupDecorator<T, object> Setup(Expression<Action<T>> expression);

    IList<IMockSetupDecorator<T>> Setups { get; }
}