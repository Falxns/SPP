using System;
using Faker;

namespace StringGeneratorLib
{
    public class StringGenerator : IPlugin
    {
        private static Random _random = new Random();
        
        public Type GetGenType()
        {
            return typeof(string);
        }

        public object Generate()
        {
            string buff = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";
            string res = "";
            for (int i = 0; i < _random.Next(256); i++)
            {
                res += buff[_random.Next(buff.Length - 1)];
            }
            return res;
        }
    }
}