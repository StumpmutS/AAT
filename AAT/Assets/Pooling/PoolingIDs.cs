using System.Collections.Generic;
using UnityEngine;

public static class PoolingIDs
{
    private static List<string> poolingIds = new List<string>();
    public static List<string> PoolingIds => poolingIds;

    public static void AddID(string id)
    {
        poolingIds.Add(id);
        string logString = "Pooling IDs: ";
        logString += poolingIds[0];
        for (int i = 1; i < poolingIds.Count; i++)
        {
            logString += ", ";
            logString += poolingIds[i];
        }
        Debug.Log(logString);
    }
}
