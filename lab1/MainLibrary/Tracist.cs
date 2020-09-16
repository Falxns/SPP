using System;
using System.Diagnostics;

namespace MainLibrary
{
    public class Tracist: ITracer
    {
        Stopwatch _stopwatch = new Stopwatch();
        private string _methodname;
        private string _classname;
        private long _elapsedtime;
        public void StartTrace()
        {
            _methodname = GetCurrentMethod();
            _classname = GetClassName();
            Console.WriteLine(_methodname);
            Console.WriteLine(_classname);
            _stopwatch.Start();
        }

        public void StopTrace()
        {
            _stopwatch.Stop(); 
            _elapsedtime = _stopwatch.ElapsedMilliseconds;
            Console.WriteLine(_elapsedtime);
        }

        public TraceResult GetTraceResult()
        {
            return new TraceResult(_methodname,_classname,_elapsedtime);
        }
        string GetCurrentMethod()
        {
            int _num = 2;
            var st = new StackTrace();
            var sf = st.GetFrame(_num);

            return sf.GetMethod().Name;
        }
        string GetClassName()
        {
            int _num = 2;
            var st = new StackTrace();
            var sf = st.GetFrame(_num);

            return sf.GetMethod()?.DeclaringType?.Name;
        }
    }
}