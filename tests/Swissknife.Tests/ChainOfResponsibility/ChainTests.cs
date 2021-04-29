using System;
using System.Collections.Generic;
using Swissknife.ChainOfResponsibility;
using Xunit;
using Xunit.Abstractions;

namespace Swissknife.Tests.ChainOfResponsibility
{
    public class ChainTests
    {
        private readonly ITestOutputHelper _output;

        public ChainTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void Addition()
        {
            // ARRANGE
            var accessors = new List<IAccessor>
            {
                new Product(),
                new Sum(),
            };
            var chain = new Chain(accessors, new TestLogger(_output));
            var req = new Request
            {
                Op = "sum", Num1 = 1, Num2 = 3,
            };
            
            // ACT
            var result = chain.Get(req);
            
            // ASSERT
            Assert.NotNull(result);
            Assert.True(result is int sum && sum == 4);
        }
        
        [Fact]
        public void Multiplication()
        {
            // ARRANGE
            var accessors = new List<IAccessor>
            {
                new Sum(),
                new Product(),
            };
            var chain = new Chain(accessors, new TestLogger(_output));
            var req = new Request
            {
                Op = "prod", Num1 = 4, Num2 = 5,
            };
            
            // ACT
            var result = chain.Get(req);
            
            // ASSERT
            Assert.NotNull(result);
            Assert.True(result is int prod && prod == 20);
        }
        
        [Fact]
        public void NullOperation()
        {
            // ARRANGE
            var accessors = new List<IAccessor> {new Explosion()};
            var chain = new Chain(accessors, new TestLogger(_output));
            var req = new Request {Num1 = 4, Num2 = 5};
            
            // ACT
            var result = chain.Get(req);
            
            // ASSERT
            Assert.Null(result);
        }
    }

    public class Sum : IAccessor
    {
        public object Get(object req)
        {
            if (req is Request request && request.Op == "sum")
            {
                return request.Num1 + request.Num2;
            }

            return null;
        }
    }
    
    public class Product : IAccessor
    {
        public object Get(object req)
        {
            if (req is Request request && request.Op == "prod")
            {
                return request.Num1 * request.Num2;
            }

            return null;
        }
    }
    
    public class Explosion : IAccessor
    {
        public object Get(object req)
        {
            throw new Exception("KABOOM");
        }
    }

    public class Request
    {
        public string Op { get; set; }
        public int Num1 { get; set; }
        public int Num2 { get; set; }
    }

    public class TestLogger : IAccessorLogger
    {
        private readonly ITestOutputHelper _output;

        public TestLogger(ITestOutputHelper output)
        {
            _output = output;
        }
        
        public void LogError(string msg)
        {
            _output.WriteLine(msg);
        }

        public void LogError(Exception ex)
        {
            _output.WriteLine($"{ex}");
        }
    }
}
