---
applyTo: "**/*Tests.cs"
---

- Use 'sut' (System Under Test) as the variable name for the main object being tested.
- Use AutoFixture.Xunit3 and NSubstitute for test data generation and mocking.
- AwesomeAssertions and xunit.v3 for unit tests.
- DisplayName attribute for test methods to provide more descriptive and unique names.
- All tests methods named like "TestX" where X is an incrementing number.
- Tests should be independent and not rely on the execution order.
- Tests should cover both typical and edge cases.
- Use `sut.Should().ThrowExactly<ExceptionType>()` to test for exceptions.