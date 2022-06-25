using Moq;
using NUnit.Framework;

namespace Rhino.Mocks.Tests;

[TestFixture]
internal sealed class FuncTests
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
    public void Expect_ForFuncWithArgAndIgnoredArguments_ShouldBehaveAsExpected()
    {
        var argument = AnyValueFactory.CreateAnyString();

        _barMock
            .Expect(x => x.FuncWithArg<string, string>(null!))
            .IgnoreArguments();

        _foo.BarFuncWithArg<string, string>(argument);

        _barMock.VerifyAllExpectations();
    }

    [Test]
    public void Expect_ForFuncWithArgAndWhenCalledAndIgnoredArguments_ShouldBehaveAsExpected()
    {
        var argument = AnyValueFactory.CreateAnyString();

        var funcCalled = false;

        _barMock
            .Expect(x => x.FuncWithArg<string, string>(null!))
            .WhenCalled(_ => funcCalled = true)
            .IgnoreArguments();

        _foo.BarFuncWithArg<string, string>(argument);

        Assert.IsTrue(funcCalled);
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

    internal class Foo
    {
        private readonly IBar _bar;

        public Foo(IBar bar)
        {
            _bar = bar;
        }

        public TResult BarFunc<TResult>() => _bar.Func<TResult>();

        public TResult BarFuncWithArg<TArgument, TResult>(TArgument arg) => _bar.FuncWithArg<TArgument, TResult>(arg);
    }

    internal interface IBar
    {
        TResult Func<TResult>();
            
        TResult FuncWithArg<TArgument, TResult>(TArgument arg);
    }
}