using UnityEngine;

[RequireComponent(typeof(TransparentPreviewController))]
public class PreviewPoolingObject : PoolingObject
{
    [SerializeField] private GameObject PreviewOfObject;
    public TransparentPreviewController Preview { get; private set; }

    private void Awake()
    {
        Preview = GetComponent<TransparentPreviewController>();
    }

    public GameObject InitiateUsage(Transform parent = null)
    {
        var instantiatedObject = Instantiate(PreviewOfObject, transform.position, transform.rotation);
        instantiatedObject.transform.parent = parent;
        instantiatedObject.transform.localScale = transform.localScale;
        return instantiatedObject;
    }
}
