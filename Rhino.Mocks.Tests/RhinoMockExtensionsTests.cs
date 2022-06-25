using Moq;
using NUnit.Framework;

namespace Rhino.Mocks.Tests;

[TestFixture]
internal sealed class RhinoMockExtensionsTests
{
    [Test]
    public void Expect_ForFuncThatHasBeenCalledAsExpected_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var expected = AnyValueFactory.CreateAnyString();
        mockedBar.Expect(x => x.Func<string>()).Return(expected);

        var actual = foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
        mockedBar.VerifyAllExpectations();
    }

    [Test]
    public void Expect_ForFuncWithArgThatHasNotBeenCalledAsExpected_ShouldFail()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var argument1 = AnyValueFactory.CreateAnyString();
        var argument2 = AnyValueFactory.CreateAnyString();
        
        var expected = AnyValueFactory.CreateAnyString();

        mockedBar.Expect(x => x.FuncWithArg<string, string>(argument1)).Return(expected);

        foo.BarFuncWithArg<string, string>(argument2);

        Assert.Throws<MockException>(() => mockedBar.VerifyAllExpectations());
    }

    [Test]
    public void Expect_ForActionThatHasBeenCalledAsExpected_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        mockedBar.Expect(x => x.Action());

        foo.BarAction();

        mockedBar.VerifyAllExpectations();
    }

    [Test]
    public void Expect_ForActionThatHasNotBeenCalledAsExpected_ShouldFail()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        mockedBar.Expect(x => x.Action());

        Assert.Throws<MockException>(() => mockedBar.VerifyAllExpectations());
    }

    [Test]
    public void Stub_ForFunc_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var expected = AnyValueFactory.CreateAnyString();
        mockedBar.Stub(x => x.Func<string>()).Return(expected);

        var actual = foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Stub_ForFuncAndItsReturnValueSetByWhenCalled_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var expected = AnyValueFactory.CreateAnyString();

        mockedBar
            .Stub(x => x.Func<string>())
            .WhenCalled(invocation => invocation.ReturnValue = expected);

        var actual = foo.BarFunc<string>();

        Assert.AreEqual(expected, actual);
    }

    [Test]
    public void Stub_ForFuncWithArgument_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var argument = AnyValueFactory.CreateAnyString();
        var expected = AnyValueFactory.CreateAnyString();

        mockedBar.Stub(x => x.FuncWithArg<string, string>(argument)).Return(expected);

        var actual = foo.BarFuncWithArg<string, string>(argument);

        Assert.AreEqual(expected, actual);
    }
        
    [Test]
    public void Stub_ForActionAndWhenCalled_ShouldBehaveAsExpected()
    {
        var mockedBar = MockRepository.GenerateMock<IBar>();

        var foo = new Foo(mockedBar);

        var actionCalled = false;

        mockedBar.Stub(x => x.Action()).WhenCalled(_ => actionCalled = true);

        foo.BarAction();

        Assert.IsTrue(actionCalled);
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