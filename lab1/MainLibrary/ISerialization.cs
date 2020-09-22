namespace MainLibrary
{
    public interface ISerialization<T>
    {
        string MakeString(T obj);
        void SaveToFile();
        void WriteToConsole();
    }
}