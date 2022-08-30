using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Utility.Scripts
{
    public static class StumpBezierHelpers
    {
        public static IEnumerable<Vector3> CreateBezier(IEnumerable<Vector3> points, int quality)
        {
            List<Vector3> returnPoints = new();
            var pointList = points.ToList();

            for (int i = 0; i <= quality; i++)
            {
                returnPoints.Add(SamplePoint(pointList, (float)i/quality));
            }

            return returnPoints;
        }

        public static Vector3 SamplePoint(List<Vector3> points, float t)
        {
            t = Mathf.Min(t, 1);
            Vector3 result = Vector3.zero;
            int n = points.Count - 1;

            for (int i = 0; i <= n; i++)
            {
                result += points[i] * ((float) StumpMath.Factorial(n) / (StumpMath.Factorial(i) * StumpMath.Factorial(n - i)) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i));
            }

            return result;
        }

        public static Vector3 SampleTangent(List<Vector3> points, float t)
        {
            t = Mathf.Min(t, 1);
            Vector3 result = Vector3.zero;
            int n = points.Count - 2;

            for (int i = 0; i <= n; i++)
            {
                result += (n + 1) * (points[i + 1] - points[i]) * ((float) StumpMath.Factorial(n) / (StumpMath.Factorial(i)
                    * StumpMath.Factorial(n - i)) * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i));
            }

            return result;
        }
    }
}
