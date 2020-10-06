using System;

namespace Faker
{
    public interface IPlugin
    {
        Object Generate();
        Type GetGenType();
    }
}