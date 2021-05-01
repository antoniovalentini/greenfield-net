using System;
using Xunit;
using Xunit.Abstractions;

namespace Executor.Tests
{
    public class ExecutorTests
    {
        private readonly ITestOutputHelper _output;

        public ExecutorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SimpleFlow()
        {
            var result = new Executor<int>(5)
                .Then(a => "empty")
                .Then(b => DateTime.Now)
                .Then(c => true)
                .Then(() => _output.WriteLine(""))
                .Catch(ex => _output.WriteLine($"Exception raised: {ex.Message}"));

            _output.WriteLine(result.Value.ToString());
        }

        [Fact]
        public void ShouldChangeReturnTypeBetweenThens()
        {
            var result = new Executor<int>(5)
                .Then(a =>
                {
                    Assert.IsType<int>(a);
                    return "string";
                })
                .Then(b =>
                {
                    Assert.IsType<string>(b);
                    return DateTime.Now;
                })
                .Then(c =>
                {
                    Assert.IsType<DateTime>(c);
                    return true;
                });

            Assert.NotNull(result);
        }

        [Fact]
        public void ShouldCatchExceptions()
        {
            var catched = false;
            var result = new Executor<int>(5)
                .Then(a => "empty")
                .Then(b => DateTime.Now)
                .Then(() => throw new Exception("test-ex"))
                .Then(() => 123)
                .Then(d => d + 1)
                .Catch(ex => catched = true);

            _output.WriteLine(result.Value.ToString());

            Assert.True(catched);
        }
    }
}
