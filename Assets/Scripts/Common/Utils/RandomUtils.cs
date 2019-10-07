using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    /// <summary>
    /// creates normal gaussian random value
    /// </summary>
    /// <param name="min"> min random value </param>
    /// <param name="max"> max random value </param>
    /// <returns></returns>
    public static float NormalGaussianRandom(float min, float max)
    {
        double mean = 0.5f;
        double stdDev = 1;

        System.Random rand = new System.Random();

        double u1 = 1.0 - rand.NextDouble();
        double u2 = 1.0 - rand.NextDouble();
        double randStdNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * System.Math.Sin(2.0 * System.Math.PI * u2);
        double randNormal = mean + stdDev * randStdNormal;

        return min + (max - min) / (float)randNormal;
    }
}
