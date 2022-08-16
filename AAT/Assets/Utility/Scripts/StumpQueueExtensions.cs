using System.Collections.Generic;

namespace Utility.Scripts
{
    public static class StumpQueueExtensions
    {
        public static void EnqueueNoDuplicates<T>(this Queue<T> queue, T item)
        {
            if (queue.Contains(item)) return;
            queue.Enqueue(item);
        }
    }
}