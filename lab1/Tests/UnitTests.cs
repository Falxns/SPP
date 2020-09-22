using ConsoleApp;
using MainLibrary;
using Xunit;

namespace Tests
{
    public class UnitTests
    {
        [Fact]
        public void InitialThreads()
        {
            ITracer tracer = new Tracist();
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Empty(traceResult.ThreadInfos);
        }
        [Fact]
        public void MethodCount()
        {
            ITracer tracer = new Tracist();
            Bar bar = new Bar(tracer);
            
            bar.InnerMethod();
            bar.InnerMethod();
            
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal(2, traceResult.ThreadInfos.Last?.Value.MethodInfos.Count);
        }
        [Fact]
        public void NestedMethods()
        {
            ITracer tracer = new Tracist();
            Foo foo = new Foo(tracer);
            
            foo.MyMethod();

            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal(1,traceResult.ThreadInfos.Last?.Value.MethodInfos.Last?.Value.MethodInfos.Count);
        }
        
        [Fact]
        public void MethodName()
        {
            ITracer tracer = new Tracist();
            Bar bar = new Bar(tracer);
            
            bar.InnerMethod();
            
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal("InnerMethod", traceResult.ThreadInfos.Last?.Value.MethodInfos.Last?.Value.MethodName);
        }
        
        [Fact]
        public void ClassName()
        {
            ITracer tracer = new Tracist();
            Bar bar = new Bar(tracer);
            
            bar.InnerMethod();
            
            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal("Bar", traceResult.ThreadInfos.Last?.Value.MethodInfos.Last?.Value.ClassName);
        }
    }
}