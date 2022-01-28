using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Compilation;
using UnityEngine;
using Random = UnityEngine.Random;

public class PoolingObject : MonoBehaviour
{
    [HideInInspector] [SerializeField] private string poolingTag;
    public string PoolingTag => poolingTag;
    
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
        poolingTag = gameObject.name;
        Debug.Log("Pooling tag created for: " + poolingTag);
    }
}
