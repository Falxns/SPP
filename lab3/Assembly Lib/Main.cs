using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Assembly_Lib
{
    public class AssemblyLib
    {
        public static AssemblyInfo GetAssemblyInfo(string path)
        {
            try
            {
                AssemblyInfo assemblyInfo = new AssemblyInfo();
                Assembly assembly = Assembly.LoadFile(path);
                foreach (Type type in assembly.GetTypes())
                {
                    string currNamespace = type.Namespace;
                    if (currNamespace != null)
                    {
                        if (!assemblyInfo.NamespaceInfos.ContainsKey(currNamespace))
                        {
                            assemblyInfo.NamespaceInfos.Add(currNamespace, new NamespaceInfo());
                        }
                        assemblyInfo.NamespaceInfos[currNamespace].DataTypeInfos.AddLast(
                            new DataTypeInfo(type.GetFields(), type.GetProperties(), type.GetMethods(), type.Name));
                    }
                }
                return assemblyInfo;
            }
            catch (Exception e)
            {
                return null;
            }
        } 
        
        public static Node BuildTree(AssemblyInfo assemblyInfo)
        {
            var root = new Node();

            foreach (var keyValue in assemblyInfo.NamespaceInfos)
            {
                string namespaceName = keyValue.Key;
                ObservableCollection<Node> namespaceList = root.Children;
                Node namespaceNode = new Node();
                namespaceNode.Name = "Namespace " + namespaceName;
                namespaceList.Add(namespaceNode);

                foreach (DataTypeInfo dataTypeInfo in keyValue.Value.DataTypeInfos)
                {
                    Node classInfo = new Node();
                    classInfo.Name = "class " + dataTypeInfo.Name;

                    var methods = dataTypeInfo.MethodInfos;
                    foreach (var method in methods)
                    {
                        var methodInfo = new Node();
                        string parametersString = null;
                        var parameters = method.GetParameters();
                        foreach (var parameter in parameters)
                        {
                            parametersString += parameter.ParameterType.Name + " " + parameter.Name + ", ";
                        }
                        if (parametersString != null && parametersString.Length > 0)
                        {
                            parametersString = parametersString.Substring(0, parametersString.Length - 2);
                        }

                        methodInfo.Name = "Method " + (method.IsPublic ? "public " : "") + (method.IsPrivate ? "private " : "") + (method.IsStatic ? "static " : "") + method.ReturnType.Name + " " + method.Name + "(" + parametersString + ")";
                        classInfo.Children.Add(methodInfo);
                    }

                    var fields = dataTypeInfo.FieldInfos;
                    foreach (FieldInfo field in fields)
                    {
                        Node fieldInfo = new Node();
                        fieldInfo.Name = "Field "  + (field.IsPublic ? "public " : "") + (field.IsPrivate ? "private " : "") + (field.IsStatic ? "static " : "") + field.FieldType.Name + " " + field.Name;
                        classInfo.Children.Add(fieldInfo);
                    }
                    
                    var properties = dataTypeInfo.PropertyInfos;
                    foreach (PropertyInfo property in properties)
                    {
                        Node propertyInfo = new Node();
                        propertyInfo.Name = "Property " + (property.CanWrite ? "set " : "") + (property.CanRead ? "get " : "") + property.PropertyType.Name + " " + property.Name;
                        classInfo.Children.Add(propertyInfo);
                    }
                    namespaceNode.Children.Add(classInfo);
                }
            }
            return root;
        }
    }

    public class AssemblyInfo
    {
        public Dictionary<string, NamespaceInfo> NamespaceInfos = new Dictionary<string, NamespaceInfo>();
    }

    public class NamespaceInfo
    {
        public LinkedList<DataTypeInfo> DataTypeInfos = new LinkedList<DataTypeInfo>();
    }

    public class DataTypeInfo
    {
        public string Name;
        public FieldInfo[] FieldInfos;
        public PropertyInfo[] PropertyInfos;
        public MethodInfo[] MethodInfos;

        public DataTypeInfo(FieldInfo[] fieldInfos, PropertyInfo[] propertyInfos, MethodInfo[] methodInfos, string name)
        {
            FieldInfos = fieldInfos;
            PropertyInfos = propertyInfos;
            MethodInfos = methodInfos;
            Name = name;
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Children { get; set; }

        public Node()
        {
            Children = new ObservableCollection<Node>();
        }
    }
}
