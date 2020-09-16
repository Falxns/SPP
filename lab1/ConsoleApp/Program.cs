using System;
using System.Diagnostics;
using System.Threading;
using MainLibrary;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Tracist tracist = new Tracist();
            //Foo _foo = new Foo(tracist);
            //_foo.MyMethod();
            Bar _bar = new Bar(tracist);
            _bar.InnerMethod();
            TraceResult res = tracist.GetTraceResult();
            res.Print();
        }
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
    
        public void MyMethod()
        {
            _tracer.StartTrace();
            
            _bar.InnerMethod();
            
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }
    
        public void InnerMethod()
        {
            _tracer.StartTrace();
            Thread.Sleep(100);
            _tracer.StopTrace();
        }
    }
}