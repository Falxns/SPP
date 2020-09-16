namespace MainLibrary
{
    public interface ITracer
    {
        void StartTrace();
        
        void StopTrace();
        
        TraceResult GetTraceResult();
    }
}