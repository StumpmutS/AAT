using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    private Dictionary<string, Stack<PoolingObject>> _inactivePoolingObjects = new();
    private Dictionary<string, HashSet<PoolingObject>> _activePoolingObjects = new();
    private Dictionary<PoolingObject, Coroutine> _destroyCoroutines = new();

    public PoolingObject CreatePoolingObject(PoolingObject poolingObject)
    {
        string id = poolingObject.PoolingTag;
        if (!_inactivePoolingObjects.ContainsKey(id)) _inactivePoolingObjects[id] = new Stack<PoolingObject>();
        if (!_activePoolingObjects.ContainsKey(id)) _activePoolingObjects[id] = new HashSet<PoolingObject>();

        if (TryGetInactive(id, out var inactive))
        {
            SetActive(id, inactive);
            return inactive;
        }

        var poolObject = Instantiate(poolingObject);
        _activePoolingObjects[id].Add(poolObject);
        poolObject.OnDeactivate += SetInactive;
        return poolObject;
    }

    private bool TryGetInactive(string id, out PoolingObject poolObj)
    {
        while (_inactivePoolingObjects[id].Count > 0)
        {
            poolObj = _inactivePoolingObjects[id].Pop();
            if (poolObj != null) return poolObj;
        }

        poolObj = null;
        return false;
    }

    private void SetActive(string id, PoolingObject poolObj)
    {
        _activePoolingObjects[id].Add(poolObj);
        if (_destroyCoroutines.ContainsKey(poolObj))
        {
            StopCoroutine(_destroyCoroutines[poolObj]);
            _destroyCoroutines.Remove(poolObj);
        }
        poolObj.Activate();
    }

    private void SetInactive(PoolingObject poolObj)
    {
        string id = poolObj.PoolingTag;
        if (!_activePoolingObjects[id].Remove(poolObj)) return;
        
        _inactivePoolingObjects[id].Push(poolObj);
        _destroyCoroutines[poolObj] = StartCoroutine(CoDestroyPoolObj(poolObj));
    }

    private IEnumerator CoDestroyPoolObj(PoolingObject poolObj)
    {
        yield return new WaitForSeconds(poolObj.DestroyTime);
        if (poolObj == null) yield break;
        if (_destroyCoroutines.ContainsKey(poolObj))
        {
            StopCoroutine(_destroyCoroutines[poolObj]);
            _destroyCoroutines.Remove(poolObj);
        }
        Destroy(poolObj.gameObject);
    }
}
