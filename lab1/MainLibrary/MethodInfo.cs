using System;
using System.Diagnostics;

namespace MainLibrary
{
    public class MethodInfo
    {
        private string _methodName;
        private string _className;
        public long MethodTime;
        public Stopwatch Watch;
        public MethodInfo(string method, string className, long time, Stopwatch watch)
        {
            _methodName = method;
            _className = className;
            MethodTime = time;
            Watch = watch;
        }
        public void Print()
        {
            Console.WriteLine(_methodName + ' ' + _className + ' ' + MethodTime);
        }
    }
}