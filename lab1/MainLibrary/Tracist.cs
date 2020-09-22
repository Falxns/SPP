using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MainLibrary
{
    public class Tracist: ITracer
    {
        private TraceResult traceResult = new TraceResult();
        ConcurrentDictionary<int,Stack<MethodInfo>> _dictionary = new ConcurrentDictionary<int, Stack<MethodInfo>>();
        public void StartTrace()
        {
            int num = Thread.CurrentThread.ManagedThreadId;
            if (!_dictionary.ContainsKey(num)) 
            {
                _dictionary.TryAdd(num, new Stack<MethodInfo>()); 
                ThreadInfo threadInfo = new ThreadInfo(num, 0, new LinkedList<MethodInfo>()); 
                traceResult.ThreadInfos.AddLast(threadInfo);
            }
            _dictionary[num].Push(new MethodInfo(GetCurrentMethod(), GetClassName(), 0, new Stopwatch())); 
            _dictionary[num].Peek().Watch.Start();
        }

        public void StopTrace()
        {
            int num = Thread.CurrentThread.ManagedThreadId;
            _dictionary[num].Peek().Watch.Stop();
            MethodInfo buff = _dictionary[num].Pop();
            buff.MethodTime = buff.Watch.ElapsedMilliseconds;
            if (_dictionary[num].Count == 0)
            {
                traceResult.ThreadInfos.Last.Value.ThreadTime += buff.MethodTime;
                traceResult.ThreadInfos.Last.Value.MethodInfos.AddLast(buff);
            }
            else
            {
                _dictionary[num].Peek().MethodInfos.AddLast(buff);
            }
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