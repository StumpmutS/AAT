using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField] private int idLength = 8;

    [HideInInspector] [SerializeField] private string id;
    public string ID => id;

    public event Action<PoolingObject> OnDeactivate = delegate { };

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
        OnDeactivate.Invoke(this);
    }

    private void OnValidate()
    {
        GenerateID();
    }

    private void GenerateID()
    {
        string newId = "";
        for (int i = 0; i < idLength; i++)
        {
            int number = UnityEngine.Random.Range(0, 9);
            newId += number;
        }
        if (!PoolingIDs.PoolingIds.Contains(id))
        {
            id = newId;
            Debug.Log($"New Pooling ID Created: {newId}");
            PoolingIDs.AddID(id);
        }
        else GenerateID();
    }
}
