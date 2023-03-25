using System.Collections.Generic;
using System.Linq;

namespace Utility.Scripts
{
    public static class StumpListExtensions
    {
        public static void Populate<T>(this List<List<T>> grid, T populateWith, int mainCount, int subCount)
        {
            while (grid.Count < mainCount)
            {
                grid.Add(new List<T>());
            }
            foreach (var list in grid)
            {
                while (list.Count < subCount)
                {
                    list.Add(populateWith);
                }
            }
        }
        
        public static void Equalize<T>(this List<T> list, int targetSize)
        {
            if (list.Count > targetSize)
            {
                list.RemoveRange(targetSize, list.Count - targetSize);
            }

            while (list.Count < targetSize)
            {
                list.Add(default);
            }
        }

        public static bool Overlapping<T>(List<T> list1, List<T> list2)
        {
            return list1.Any(list2.Contains);
        }
    }
}
