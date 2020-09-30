using System;
using System.Collections.Generic;
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
            {typeof(double), () => GenerateDouble()}
        };
        private static Random _random = new Random();
        public Faker()
        {}
        
        public T Create<T>()
        {
            Type type = typeof(T);
            return (T)CreateRec(type);
        }

        private Object CreateRec(Type type)
        {
            ConstructorInfo constructor = SelectConstructor(type);
            if (constructor == null) return null;
            Object[] param = GenerateParams(constructor);
            Object obj = constructor.Invoke(param);
            
            SetFields(obj,type);
            SetProperties(obj,type);
            
            return obj;
        }

        private ConstructorInfo SelectConstructor(Type type)
        {
            List<ConstructorInfo> constructors = new List<ConstructorInfo>();
            List<int> lengthsConstructors = new List<int>();

            foreach (ConstructorInfo constructorInfo in type.GetConstructors())
            {
                constructors.Add(constructorInfo);
                lengthsConstructors.Add(constructorInfo.GetParameters().Length);
            }

            int[] numbers = lengthsConstructors.ToArray();
            if (numbers.Length == 0)
            {
                return null;
            }

            int minValue = numbers.Min();
            int minIndex = numbers.ToList().IndexOf(minValue);
            return constructors[minIndex];
        }

        private Object[] GenerateParams(ConstructorInfo constructorInfo)
        {
            LinkedList<Object> result = new LinkedList<object>();
            foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
            {
                Type parameterType = parameterInfo.ParameterType;
                result.AddLast(Types[parameterType]());
            }

            return result.ToArray();
        }

        private void SetFields(Object obj, Type type)
        {
            foreach (FieldInfo fieldInfo in type.GetFields())
            {
                Type fieldType = fieldInfo.FieldType;
                fieldInfo.SetValue(obj, Types[fieldType]());
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
                propertyInfo.SetValue(obj, Types[propertyType]());
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
    }
}