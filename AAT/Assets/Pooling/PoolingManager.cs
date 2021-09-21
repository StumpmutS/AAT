using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    private static PoolingManager instance;
    public static PoolingManager Instance => instance;

    private Dictionary<string, List<PoolingObject>> inactivePoolingObjects = new Dictionary<string, List<PoolingObject>>();
    private Dictionary<string, List<PoolingObject>> activePoolingObjects = new Dictionary<string, List<PoolingObject>>();

    private void Awake()
    {
        instance = this;
    }

    public PoolingObject CreatePoolingObject(PoolingObject poolingObject)
    {
        string id = poolingObject.ID;
        if (!activePoolingObjects.ContainsKey(id)) activePoolingObjects[id] = new List<PoolingObject>();
        if (!inactivePoolingObjects.ContainsKey(id)) inactivePoolingObjects[id] = new List<PoolingObject>();

        if (inactivePoolingObjects[id].Count > 0)
        {
            var poolObj = inactivePoolingObjects[id][0];
            activePoolingObjects[id].Add(poolObj);
            inactivePoolingObjects[id].Remove(poolObj);
            poolObj.Activate();
            return poolObj;
        }

        var poolObject = Instantiate(poolingObject);
        activePoolingObjects[id].Add(poolObject);
        poolObject.OnDeactivate += SetInactive;
        return poolObject;
    }

    private void SetInactive(PoolingObject poolingObject)
    {
        string id = poolingObject.ID;
        if (activePoolingObjects[id].Remove(poolingObject))
        {
            inactivePoolingObjects[id].Add(poolingObject);
        }
    }
}
