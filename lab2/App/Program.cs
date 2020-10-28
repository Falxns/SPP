using System;
using System.Collections.Generic;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Faker.Faker faker = new Faker.Faker();
            Foo foo = faker.Create<Foo>();
            Bar bar = faker.Create<Bar>();
            A a = faker.Create<A>();
        }
    }

    class Foo
    {
        private int IntegerNum;
        private float FloatNum;
        public double DoubleNum;
        private long _longNum;
        public byte ByteNum;
        private bool _boolValue;
        private char _charValue;
        public string StringValue;
        public DateTime DataValue;
        public List<int> ListNum;
        private List<char> _charList;
        private List<bool> _boolList;
        public Bar bar;
        public Dictionary<int, int> Dictionary;

        public Foo(int Integer, float Float)
        {
            IntegerNum = Integer;
            FloatNum = Float;
        }

        public Foo(long Long, char Char, bool Bool, List<bool> BoolList)
        {
            _longNum = Long;
            _charValue = Char;
            _boolValue = Bool;
            _boolList = BoolList;
        }

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

        public List<char> CharList
        {
            set => _charList = value;
        }
    }

    class Bar
    {
        public int IntegerNum;
        public A a;
        
        public Bar()
        {}
    }

    class A
    {
        public B b;
        public B b1;
        public B b2;
    }

    class B
    {
        public A a;
    }
    class C
    {
        public A a;
    }
}