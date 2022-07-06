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
        instantiatedObject.transform.parent = parent;
        instantiatedObject.transform.localScale = transform.localScale;
        return instantiatedObject;
    }

    protected abstract GameObject CreateObject();
}
