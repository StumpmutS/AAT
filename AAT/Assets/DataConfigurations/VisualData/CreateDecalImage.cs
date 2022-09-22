using UnityEngine;

[CreateAssetMenu(menuName = "Visuals/Visual Components/Create Decal Image Component")]
public class CreateDecalImage : VisualComponent
{
    [SerializeField] private DecalImage decalImage;
    
    public override void ActivateComponent(VisualInfo info)
    {
        var created = PoolingManager.Instance.CreatePoolingObject(decalImage.PoolObj).GetComponent<DecalImage>();
        created.Activate(info);
    }
}