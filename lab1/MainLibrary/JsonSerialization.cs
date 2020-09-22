using System;
using System.IO;
using Newtonsoft.Json;

namespace MainLibrary
{
    public class JsonSerialization : ISerialization<TraceResult>
    {
        private const string FILE_PATH = "json_out.txt";
        public string Json;
        public string MakeString(TraceResult obj)
        {
            Json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return Json;
        }

        public void SaveToFile()
        {
            using (FileStream fileStream = File.Create(FILE_PATH))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(Json);
                fileStream.Write(array);
            }
        }

        public void WriteToConsole()
        {
            Console.WriteLine(Json);
        }
    }
}