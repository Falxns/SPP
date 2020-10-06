using System;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Faker.Faker faker = new Faker.Faker();
            Foo foo = faker.Create<Foo>();
        }
    }

    class Foo
    {
        public int IntegerNum;
        public float FloatNum;
        public double DoubleNum;
        private long _longNum;
        public byte ByteNum;
        private bool _boolValue;
        private char _charValue;
        public string StringValue;

        public Foo()
        {}

        public long LongNum
        {
            set => _longNum = value;
        }

        public bool BoolValue
        {
            get => _boolValue;
            set => _boolValue = value;
        }

        public char CharValue
        {
            set => _charValue = value;
        }
    }
}