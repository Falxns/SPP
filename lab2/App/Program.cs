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

        public Foo()
        {}
    }
}