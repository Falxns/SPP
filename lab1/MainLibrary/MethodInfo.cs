using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MainLibrary
{
    public class MethodInfo
    {
        public string MethodName;
        public string ClassName;
        public long MethodTime;
        [NonSerialized]public Stopwatch Watch;
        public LinkedList<MethodInfo> MethodInfos = new LinkedList<MethodInfo>();
        public MethodInfo(string method, string className, long time, Stopwatch watch)
        {
            MethodName = method;
            ClassName = className;
            MethodTime = time;
            Watch = watch;
        }
    }
}