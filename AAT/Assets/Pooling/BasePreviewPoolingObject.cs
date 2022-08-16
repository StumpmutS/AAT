using UnityEngine;

[RequireComponent(typeof(TransparentPreviewController))]
public abstract class BasePreviewPoolingObject : PoolingObject
{
    public TransparentPreviewController Preview { get; private set; }

    private void Awake()
    {
        Preview = GetComponent<TransparentPreviewController>();
    }

    public GameObject InitiateUsage(Transform parent = null)
    {
        var instantiatedObject = CreateObject();
        if (instantiatedObject == null) return null;
        
        instantiatedObject.transform.parent = parent;
        instantiatedObject.transform.localScale = transform.localScale;
        return instantiatedObject;
    }

    protected abstract GameObject CreateObject();
}
