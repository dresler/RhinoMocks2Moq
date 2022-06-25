using Moq;
using NUnit.Framework;

namespace Rhino.Mocks.Tests;

[TestFixture]
internal sealed class ActionTests
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
    public void Expect_ForActionWithArgAndIgnoredArguments_ShouldBehaveAsExpected()
    {
        var argument = AnyValueFactory.CreateAnyString();

        _barMock
            .Expect(x => x.ActionWithArg<string>(null!))
            .IgnoreArguments();

        _foo.BarActionWithArg(argument);

        _barMock.VerifyAllExpectations();
    }
    
    [Test]
    public void Expect_ForActionWithArgAndWhenCalledAndIgnoredArguments_ShouldBehaveAsExpected()
    {
        var argument = AnyValueFactory.CreateAnyString();

        var actionCalled = false;

        _barMock
            .Expect(x => x.ActionWithArg<string>(null!))
            .WhenCalled(_ => actionCalled = true)
            .IgnoreArguments();

        _foo.BarActionWithArg(argument);

        Assert.IsTrue(actionCalled);
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

        public void BarAction() => _bar.Action();
        
        public void BarActionWithArg<TArgument>(TArgument arg) => _bar.ActionWithArg(arg);
    }

    internal interface IBar
    {
        void Action();
        
        void ActionWithArg<TArgument>(TArgument arg);
    }
}