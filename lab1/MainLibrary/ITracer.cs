using System.Collections.Generic;

namespace MainLibrary
{
    public interface ITracer
    {
        void StartTrace();
        
        void StopTrace();
        
        TraceResult GetTraceResult();
    }
}