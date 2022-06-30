using Moq;
using NUnit.Framework;

namespace Rhino.Mocks.Tests;

[TestFixture]
internal sealed class PropertyTests
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
    public void Expect_ForGetterThatHasBeenCalledAsExpected_ShouldBehaveAsExpected()
    {
        _barMock.Expect(x => x.Property);

        var _ = _foo.BarProperty;

        _barMock.VerifyAllExpectations();
    }
    
    [Test]
    public void Expect_ForGetterThatHasNotBeenCalledAsExpected_ShouldFail()
    {
        _barMock.Expect(x => x.Property);

        Assert.Throws<MockException>(() => _barMock.VerifyAllExpectations());
    }

    [Test]
    public void Stub_ForGetterAndWhenCalled_ShouldBehaveAsExpected()
    {
        var getterCalled = false;

        _barMock.Stub(x => x.Property).WhenCalled(_ => getterCalled = true);

        var _ = _foo.BarProperty;

        Assert.IsTrue(getterCalled);
    }

    [Test]
    public void Stub_ForGetterThrowingException_ShouldBehaveAsExpected()
    {
        var expectedException = new Exception();

        _barMock.Stub(x => x.Property).Throw(expectedException);

        var actualException = Assert.Throws<Exception>(() =>
        {
            var _ = _foo.BarProperty;
        });

        Assert.AreEqual(expectedException, actualException);
    }

    [Test]
    public void Stub_ForGetterStub_ShouldReturnExpectedPropertyValue()
    {
        var expected = AnyValueFactory.CreateAnyString();

        _barMock.Stub(x => x.Property).Return(expected);

        Assert.AreEqual(expected, _foo.BarProperty);
    }

    // There is no support for mocking setters.

    internal class Foo
    {
        private readonly IBar _bar;

        public Foo(IBar bar)
        {
            _bar = bar;
        }

        public string BarProperty
        {
            get => _bar.Property;
            set => _bar.Property = value;
        }
    }

    internal interface IBar
    {
        string Property { get; set; }
    }
}