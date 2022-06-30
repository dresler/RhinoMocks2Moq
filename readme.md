# RhinoMocks2Moq

* Do you have planty of unit tests written with usage of Rhino Mocks?
* Do you need to switch to Moq, i.e. in case of migrating to a newer version of .Net?
* Do you not want to rewrite all the unit tests?

If answers to all the questions is YES then you can try this library. 

RhinoMocks2Moq fakes Rhino Mocks mocking logic by adopting it to Moq logic. Not all methods of Rhino Mocks are covered for now. However the scope of supported parts is getting bigger.

All namespaces and type and member names are compatible with Rhino Mocks names.
Only you need to do is replacing Rhino Mocks library by this library.i

## List of supported Rhino Mocks logic

### Creating mocks
```
MockRepository.GenerateMock<T>
```

### Expectations

```
mock
  .Stub(m => m.Func(...))
  .Returns(...)
```
```
mock
  .Stub(m => m.Func(...))
  .IgnoreArguments()
  .Returns(...)
```
```
mock
  .Stub(m => m.Property)
  .Returns(...)
```
```
mock
  .Stub(m => m.Method(...))
  .Throws(...)
```
```
mock.Expect(m => m.Method(...))
...
mock.VerifyAllExpectations()
```

### Invocation callback

```
mock
  .Expect(m => m.Action(...))
  .WhenCalled(invocation => ...)
```
```
mock
  .Stub(m => m.Func(...))
  .WhenCalled(invocation => ...)
  .Returns(...)
```