using System;
using System.Collections.Generic;
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
            Bar _bar = new Bar(tracist);
            Foo _foo = new Foo(tracist);

            Thread myThread1 = new Thread(_foo.MyMethod);
            myThread1.Start();
            myThread1.Join();
            
            _foo.MyMethod();
            _bar.InnerMethod();
            
            Thread myThread2 = new Thread(_bar.InnerMethod);
            myThread2.Start();
            myThread2.Join();

            TraceResult res = tracist.GetTraceResult();
            ISerialization<TraceResult> Serialization = new JsonSerialization();
            Serialization.MakeString(res);
            Serialization.WriteToConsole();
            Serialization.SaveToFile();
            Serialization = new XmlSerialization();
            Serialization.MakeString(res);
            Serialization.WriteToConsole();
            Serialization.SaveToFile();
        }
    }
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        public Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }
        public void MyMethod()
        {
            _tracer.StartTrace();
            
            Thread.Sleep(50);
            _bar.InnerMethod();

            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        public Bar(ITracer tracer)
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