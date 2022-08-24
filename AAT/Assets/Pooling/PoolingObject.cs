using System;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField] private float destroyTime = 30;
    public float DestroyTime => destroyTime;
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
