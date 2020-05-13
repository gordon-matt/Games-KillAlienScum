using System;
using System.Collections.Generic;
using System.Linq;

public static class RandomExtensions
{
    public static T NextFrom<T>(this Random random, IEnumerable<T> enumerable)
    {
        return enumerable.ElementAt(random.Next(0, enumerable.Count() - 1));
    }
}