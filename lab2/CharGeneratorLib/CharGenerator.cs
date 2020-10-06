using System;
using Faker;

namespace CharGeneratorLib
{
    public class CharGenerator : IPlugin
    {
        private static Random _random = new Random();
        
        public Type GetGenType()
        {
            return typeof(char);
        }

        public object Generate()
        {
            string buff = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            return buff[_random.Next(buff.Length - 1)];
        }
    }
}