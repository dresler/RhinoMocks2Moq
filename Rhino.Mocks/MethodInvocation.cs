using System.Reflection;
using Moq;

namespace Rhino.Mocks;

public sealed class MethodInvocation
{
    private readonly IInvocation _invocation;
    private readonly Action<object> _setReturnValueHandler;

    internal MethodInvocation(IInvocation invocation, Action<object> setReturnValueHandler)
    {
        _invocation = invocation;
        _setReturnValueHandler = setReturnValueHandler;
    }

    public object[] Arguments => this._invocation.Arguments.ToArray();

    public MethodInfo Method => this._invocation.Method;

    public object ReturnValue
    {
        get => _invocation.ReturnValue;
        set => _setReturnValueHandler(value);
    }
}