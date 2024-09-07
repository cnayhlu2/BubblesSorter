using System;
using System.Collections.Generic;

namespace TestGame
{
    public static class Utilities
    {
        public static T GetRandomValue<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RandomEnumValue<T>() where T : Enum
        {
            var v = Enum.GetValues(typeof(T));
            return (T) v.GetValue(UnityEngine.Random.Range(0, v.Length));
        }
    }
}