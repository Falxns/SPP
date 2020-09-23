using System.Threading;
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

            for (int i = 0; i < 100; i++)
            {
                bar.InnerMethod();
            }

            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal(100, traceResult.ThreadInfos.Last?.Value.MethodInfos.Count);
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

        [Fact]
        public void ThreadCount()
        {
            ITracer tracer = new Tracist();
            Foo foo = new Foo(tracer);
            Bar bar = new Bar(tracer);
            
            bar.InnerMethod();
            
            Thread myThread1 = new Thread(foo.MyMethod);
            myThread1.Start();
            myThread1.Join();
            
            Thread myThread2 = new Thread(bar.InnerMethod);
            myThread2.Start();
            myThread2.Join();

            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal(3,traceResult.ThreadInfos.Count);
        }
        [Fact]
        public void ThreadCountMax()
        {
            ITracer tracer = new Tracist();
            
            Bar bar = new Bar(tracer);
            
            bar.InnerMethod();
            
            for (int i = 0; i < 5000; i++)
            {
                Thread myThread1 = new Thread(bar.InnerMethod);
                myThread1.Start();
                //myThread1.Join();
            }

            TraceResult traceResult = tracer.GetTraceResult();
            Assert.Equal(1001,traceResult.ThreadInfos.Count);
        }
    }
}