
using System.Collections.Generic;
using UnityEngine;

namespace Tank.Helpers
{
    public static class Exp
    {
        private static readonly Dictionary<int, float> cache = new Dictionary<int, float>();

        public static float GetPower(int power)
        {
            if (cache.ContainsKey(power) == false)
            {
                cache.Add(power, Mathf.Exp(power));
            }

            return cache[power];
        }

        public static float GetInversePower(int power)
        {
            if (cache.ContainsKey(power) == false)
            {
                cache.Add(power, 1f / Mathf.Exp(power));
            }

            return cache[power];
        }
    }
}
