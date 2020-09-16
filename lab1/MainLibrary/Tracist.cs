using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace MainLibrary
{
    public class Tracist: ITracer
    {
        private TraceResult traceResult = new TraceResult(); 
        Stack<MethodInfo> _stackInfo = new Stack<MethodInfo>();
        public void StartTrace()
        {
            _stackInfo.Push(new MethodInfo(GetCurrentMethod(),GetClassName(),0,new Stopwatch()));
            _stackInfo.Peek().watch.Start();
        }

        public void StopTrace()
        {
            _stackInfo.Peek().watch.Stop();
            _stackInfo.Peek().methodTime = _stackInfo.Peek().watch.ElapsedMilliseconds;
            traceResult.MethodInfos.AddLast(_stackInfo.Peek());
            _stackInfo.Pop();
        }

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }
        string GetCurrentMethod()
        {
            int num = 2;
            var st = new StackTrace();
            var sf = st.GetFrame(num);

            return sf.GetMethod().Name;
        }
        string GetClassName()
        {
            int num = 2;
            var st = new StackTrace();
            var sf = st.GetFrame(num);

            return sf.GetMethod()?.DeclaringType?.Name;
        }
    }
}