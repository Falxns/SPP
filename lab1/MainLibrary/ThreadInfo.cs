using System;
using System.Collections.Generic;

namespace MainLibrary
{
    public class ThreadInfo
    {
        public int ThreadId;
        public long ThreadTime;
        public LinkedList<MethodInfo> MethodInfos;
        public ThreadInfo(int threadId,long threadTime, LinkedList<MethodInfo> methodInfos)
        {
            ThreadId = threadId;
            ThreadTime = threadTime;
            MethodInfos = methodInfos;
        }
    }
}