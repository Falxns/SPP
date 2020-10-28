using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Faker
{
    public class Faker
    {
        public static Dictionary<Type, Func<Object>> Types = new Dictionary<Type, Func<object>>
        {
            {typeof(int), () => GenerateInt()},
            {typeof(float), () => GenerateFloat()},
            {typeof(double), () => GenerateDouble()},
            {typeof(long), () => GenerateLong()},
            {typeof(byte), () => GenerateByte()},
            {typeof(bool), () => GenerateBool()},
            {typeof(DateTime), () => GenerateDateTime()},
            {typeof(List<Object>), () => GenerateList(typeof(Type))}
        };
        private static Random _random = new Random();
        private HashSet<Type> _used = new HashSet<Type>();
        
        private string _pluginsPath = "/Users/falxns/Downloads/СПП/lab2/App/Plugins";
            
        public Faker()
        {
            InstallPlugins();
        }
        
        public T Create<T>()
        {
            Type type = typeof(T);
            return (T)CreateRec(type);
        }

        private Object CreateRec(Type type)
        {
            if (_used.Contains(type))
            {
                // throw new Exception("Cyclical dependence");
                return null;
            }
            _used.Add(type);
            
            ConstructorInfo constructor = SelectConstructor(type);
            if (constructor == null)
            {
                _used.Remove(type);
                return null; 
            }
            Object[] param = GenerateParams(constructor);
            Object obj = constructor.Invoke(param);
            
            SetFields(obj,type);
            SetProperties(obj,type);

            _used.Remove(type);
            return obj;
        }

        private ConstructorInfo SelectConstructor(Type type)
        {
            List<ConstructorInfo> constructors = new List<ConstructorInfo>();
            List<int> lengthsConstructors = new List<int>();

            foreach (ConstructorInfo constructorInfo in type.GetConstructors())
            {
                if (constructorInfo.GetParameters().All(info => Types.ContainsKey(info.ParameterType) 
                || IsList(info.ParameterType)
                || IsDTO(info.ParameterType)))
                {
                    constructors.Add(constructorInfo);
                    lengthsConstructors.Add(constructorInfo.GetParameters().Length);
                }
            }

            int[] numbers = lengthsConstructors.ToArray();
            if (numbers.Length == 0)
            {
                return null;
            }

            int maxValue = numbers.Max();
            int maxIndex = numbers.ToList().IndexOf(maxValue);
            return constructors[maxIndex];
        }

        private Object[] GenerateParams(ConstructorInfo constructorInfo)
        {
            LinkedList<Object> result = new LinkedList<object>();
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                Type parameterType = parameterInfo.ParameterType;
                if (!Types.ContainsKey(parameterType))
                {
                    if (IsList(parameterType))
                    {
                        SetListParameter(result, parameterType);
                    }
                    else if (IsDTO(parameterType))
                    {
                        result.AddLast(CreateRec(parameterType));
                    }
                }
                else
                {
                    result.AddLast(Types[parameterType]());
                }
            }

            return result.ToArray();
        }

        private void SetFields(Object obj, Type type)
        {
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                Type fieldType = fieldInfo.FieldType;
                if (!Types.ContainsKey(fieldType))
                {
                    if (IsList(fieldType))
                    {
                        SetListField(obj, fieldInfo);
                    }
                    else if (IsDTO(fieldType))
                    {
                        fieldInfo.SetValue(obj, CreateRec(fieldType));
                    }
                }
                else
                {
                    fieldInfo.SetValue(obj, Types[fieldType]());
                }
            }
        }

        private void SetProperties(Object obj, Type type)
        {
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (!propertyInfo.CanWrite)
                {
                    continue;
                }
                Type propertyType = propertyInfo.PropertyType;
                if (!Types.ContainsKey(propertyType))
                {
                    if (IsList(propertyType))
                    {
                        SetListProperty(obj, propertyInfo);
                    }
                    else if (IsDTO(propertyType))
                    {
                        propertyInfo.SetValue(obj, CreateRec(propertyType));
                    }
                }
                else
                {
                    propertyInfo.SetValue(obj, Types[propertyType]());
                }
            }
        }

        private static bool IsDTO(Type type)
        {
            return !type.IsValueType && !type.IsGenericType && type.IsClass;
        }
        
        private static bool IsList(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>) && Types.ContainsKey(type.GetGenericArguments()[0]);
        }
        
        private void InstallPlugins()
        {
            DirectoryInfo pluginDir = new DirectoryInfo(_pluginsPath);
            if (!pluginDir.Exists)
                pluginDir.Create();
            
            string[] pluginFiles = Directory.GetFiles(_pluginsPath, "*.dll");
            foreach (string file in pluginFiles)
            {
                Assembly asm = Assembly.LoadFrom(file);
                IEnumerable<Type> types = asm.GetTypes().
                    Where(t => t.GetInterfaces().Any(i => i.FullName == typeof(IPlugin).FullName));

                foreach (Type type in types)
                {           
                    IPlugin plugin = asm.CreateInstance(type.FullName) as IPlugin;
                    Types.Add(plugin.GetGenType(), () => plugin.Generate());
                }
            }
        }

        private static int GenerateInt()
        {
            return _random.Next(Int32.MaxValue);
        }

        private static float GenerateFloat()
        {
            double res = _random.NextDouble() * _random.Next(1000);
            return (float)res;
        }
        
        private static double GenerateDouble()
        {
            return _random.NextDouble();
        }
        
        private static long GenerateLong()
        {
            byte[] buff = new byte[8];
            _random.NextBytes(buff);
            return BitConverter.ToInt64(buff, 0);
        }
        
        private static byte GenerateByte()
        {
            byte[] buff = new byte[1];
            _random.NextBytes(buff);
            return buff[0];
        }
        
        private static bool GenerateBool()
        {
            return _random.Next(2) == 1;
        }

        private static DateTime GenerateDateTime()
        {
            DateTime start = new DateTime(2007, 1, 1, 1,1,1);
            return start.AddSeconds(_random.Next((DateTime.Today - start).Seconds))
                .AddMinutes(_random.Next((DateTime.Today - start).Minutes))
                .AddHours(_random.Next((DateTime.Today - start).Hours))
                .AddDays(_random.Next((DateTime.Today - start).Days));
        }

        private static List<Object> GenerateList(Type type)
        {
            int len = _random.Next(1, 6);
            List<Object> res = new List<Object>();
            if (Types.ContainsKey(type))
            {
                for (int i = 0; i < len; i++)
                {
                    res.Add(Types[type]());
                }
            }
            return res;
        }

        private static void SetListParameter(LinkedList<Object> result, Type type)
        {
            Type listType = type.GetGenericArguments()[0];
            List<Object> res = GenerateList(listType);
            if (res[0] is int)
            {
                result.AddLast(res.Cast<int>().ToList());
            } else if (res[0] is bool)
            {
                result.AddLast(res.Cast<bool>().ToList());
            } else if (res[0] is char)
            {
                result.AddLast(res.Cast<char>().ToList());
            }
        }
        
        private static void SetListField(Object obj, FieldInfo fieldInfo)
        {
            Type listType = fieldInfo.FieldType.GetGenericArguments()[0];
            List<Object> res = GenerateList(listType);
            if (res[0] is int)
            {
                fieldInfo.SetValue(obj, res.Cast<int>().ToList());
            } else if (res[0] is bool)
            {
                fieldInfo.SetValue(obj, res.Cast<bool>().ToList());
            } else if (res[0] is char)
            {
                fieldInfo.SetValue(obj, res.Cast<char>().ToList());
            }
        }
        
        private static void SetListProperty(Object obj, PropertyInfo propertyInfo)
        {
            Type listType = propertyInfo.PropertyType.GetGenericArguments()[0];
            List<Object> res = GenerateList(listType);
            if (res[0] is int)
            {
                propertyInfo.SetValue(obj, res.Cast<int>().ToList());
            } else if (res[0] is bool)
            {
                propertyInfo.SetValue(obj, res.Cast<bool>().ToList());
            } else if (res[0] is char)
            {
                propertyInfo.SetValue(obj, res.Cast<char>().ToList());
            }
        }
    }
}