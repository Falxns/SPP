using System;
using System.Runtime.InteropServices;

namespace MainLibrary
{
    public class TraceResult
    {
        private string _methodName;
        private string _className;
        private long _methodTime;

        public TraceResult(string method, string className, long time)
        {
            _methodName = method;
            _className = className;
            _methodTime = time;
        }
        public void Print()
        {
            Console.WriteLine(_methodName);
            Console.WriteLine(_className);
            Console.WriteLine(_methodTime);
        }
    }
}