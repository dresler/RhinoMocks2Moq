using Moq;
using NUnit.Framework;

namespace Rhino.Mocks.Tests;

[TestFixture]
internal sealed class RhinoMockExtensionsTests
{
    private IBar _barMock = null!;
    private Foo _foo = null!;

    [SetUp]
    public void SetUp()
    {
        _barMock = MockRepository.GenerateMock<IBar>();
        _foo = new Foo(_barMock);
    }

    [Test]
    public void Expect_ForFuncThatHasBeenCalledAsExpected_ShouldBehaveAsExpected()
    {
        var expected = AnyValueFactory.CreateAnyString();
        _barMock.Expect(x => x.Func<string>()).Return(expected);

        var actual = _foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
        _barMock.VerifyAllExpectations();
    }

    [Test]
    public void Expect_ForFuncWithArgThatHasNotBeenCalledAsExpected_ShouldFail()
    {
        var argument1 = AnyValueFactory.CreateAnyString();
        var argument2 = AnyValueFactory.CreateAnyString();
        
        var expected = AnyValueFactory.CreateAnyString();

        _barMock.Expect(x => x.FuncWithArg<string, string>(argument1)).Return(expected);

        _foo.BarFuncWithArg<string, string>(argument2);

        Assert.Throws<MockException>(() => _barMock.VerifyAllExpectations());
    }

    [Test]
    public void Expect_ForActionThatHasBeenCalledAsExpected_ShouldBehaveAsExpected()
    {
        _barMock.Expect(x => x.Action());

        _foo.BarAction();

        _barMock.VerifyAllExpectations();
    }

    [Test]
    public void Expect_ForActionThatHasNotBeenCalledAsExpected_ShouldFail()
    {
        _barMock.Expect(x => x.Action());

        Assert.Throws<MockException>(() => _barMock.VerifyAllExpectations());
    }

    [Test]
    public void Stub_ForFunc_ShouldBehaveAsExpected()
    {
        var expected = AnyValueFactory.CreateAnyString();
        _barMock.Stub(x => x.Func<string>()).Return(expected);

        var actual = _foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Stub_ForFuncAndItsReturnValueSetByWhenCalled_ShouldBehaveAsExpected()
    {
        var expected = AnyValueFactory.CreateAnyString();

        _barMock
            .Stub(x => x.Func<string>())
            .WhenCalled(invocation => invocation.ReturnValue = expected);

        var actual = _foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Stub_ForFuncWithArgument_ShouldBehaveAsExpected()
    {
        var argument = AnyValueFactory.CreateAnyString();
        var expected = AnyValueFactory.CreateAnyString();

        _barMock.Stub(x => x.FuncWithArg<string, string>(argument)).Return(expected);

        var actual = _foo.BarFuncWithArg<string, string>(argument);

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Stub_ForFuncThrowingException_ShouldBehaveAsExpected()
    {
        var expectedException = new Exception();

        _barMock.Stub(x => x.Func<string>()).Throw(expectedException);

        var actualException = Assert.Throws<Exception>(() => _foo.BarFunc<string>());

        Assert.AreEqual(expectedException, actualException);
    }

    [Test]
    public void Stub_ForActionAndWhenCalled_ShouldBehaveAsExpected()
    {
        var actionCalled = false;

        _barMock.Stub(x => x.Action()).WhenCalled(_ => actionCalled = true);

        _foo.BarAction();

        Assert.IsTrue(actionCalled);
    }

    [Test]
    public void Stub_ForActionThrowingException_ShouldBehaveAsExpected()
    {
        var expectedException = new Exception();

        _barMock.Stub(x => x.Action()).Throw(expectedException);

        var actualException = Assert.Throws<Exception>(_foo.BarAction);

        Assert.AreEqual(expectedException, actualException);
    }

    internal class Foo
    {
        private readonly IBar _bar;

        public Foo(IBar bar)
        {
            _bar = bar;
        }

        public TResult BarFunc<TResult>() => _bar.Func<TResult>();

        public TResult BarFuncWithArg<TArgument, TResult>(TArgument arg) => _bar.FuncWithArg<TArgument, TResult>(arg);
            
        public void BarAction() => _bar.Action();
    }

    internal interface IBar
    {
        TResult Func<TResult>();
            
        TResult FuncWithArg<TArgument, TResult>(TArgument arg);

        void Action();
    }
}