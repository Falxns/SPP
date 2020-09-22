using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace MainLibrary
{
    public class XmlSerialization : ISerialization<TraceResult>
    {
        private const string FILE_PATH = "xml_out.txt";
        private XDocument _xDocument = new XDocument();
        public string MakeString(TraceResult obj)
        {
            XElement root = new XElement("root");
            foreach (ThreadInfo threadInfo in obj.ThreadInfos)
            {
                XElement thread = new XElement("thread");
                XAttribute threadId = new XAttribute("id", threadInfo.ThreadId);
                XAttribute threadTime = new XAttribute("time", threadInfo.ThreadTime);
                thread.Add(threadId, threadTime);

                AddMethods(thread, threadInfo.MethodInfos);
                
                root.Add(thread);
            }
            _xDocument.Add(root);

            return _xDocument.ToString();
        }
        private static void AddMethods(XElement xElement, LinkedList<MethodInfo> methodInfos)
        {
            foreach (MethodInfo methodInfo in methodInfos)
            {
                XElement method = new XElement("method");
                
                XAttribute methodName = new XAttribute("name", methodInfo.MethodName);
                XAttribute methodClass = new XAttribute("class", methodInfo.ClassName);
                XAttribute methodTime = new XAttribute("time", methodInfo.MethodTime);
                
                method.Add(methodName, methodClass, methodTime);
                
                AddMethods(method, methodInfo.MethodInfos);
                
                xElement.Add(method);
            }
        }
        public void SaveToFile()
        {
            using (FileStream fileStream = File.Create(FILE_PATH))
            {
                _xDocument.Save(fileStream);
            }
        }

        public void WriteToConsole()
        {
            Console.WriteLine(_xDocument.ToString());
        }
    }
}