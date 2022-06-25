using System.Linq.Expressions;

namespace Rhino.Mocks.Interfaces;

internal interface IMethodSetupDecorator<T>
{
    void Replay(Expression? expression = null);
}

internal interface IMethodSetupDecorator<T, TResult> : IMethodSetupDecorator<T>, IMethodOptions<TResult>
{
    IMethodSetupDecorator<T, TResult> Verifiable();
}