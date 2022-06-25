using System.Linq.Expressions;

namespace Rhino.Mocks.Interfaces;

internal interface IMockSetupDecorator<T>
{
    void Replay(Expression<Action<T>>? expression = null);
}

internal interface IMockSetupDecorator<T,R> : IMockSetupDecorator<T>, IMethodOptions<R>
{
    IMockSetupDecorator<T, R> Verifiable();
}