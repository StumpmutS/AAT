using UnityEngine;

[CreateAssetMenu(menuName = "Visuals/Visual Components/Create Pooling Object Component")]
public class CreatePoolingObjectComponent : VisualComponent
{
    [SerializeField] private PoolingObject poolObj;
    
    public override void ActivateComponent(VisualInfo info)
    {
        var created = PoolingManager.Instance.CreatePoolingObject(poolObj);
        created.transform.position = Vector3.one * Random.Range(-info.Randomize, info.Randomize) + info.Position + info.Direction.normalized * info.DirectionMultiplier;
        created.transform.rotation = info.Rotation;
    }
}