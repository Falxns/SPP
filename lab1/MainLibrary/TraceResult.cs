using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MainLibrary
{
    public class TraceResult
    {
        public LinkedList<ThreadInfo> ThreadInfos = new LinkedList<ThreadInfo>();

        public void Print()
        {
            foreach (var buff in ThreadInfos)
            {
                buff.Print();
            }
        }
    }
}