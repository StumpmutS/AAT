using UnityEngine;

public class PlaceableWallController : MonoBehaviour
{
    [SerializeField] private DimensionsContainer dimensionsContainer;
    public DimensionsContainer DimensionsContainer => dimensionsContainer;
    [SerializeField] private PreviewPoolingObject wallVisuals;
    public PreviewPoolingObject WallVisuals => wallVisuals;
    [SerializeField] private BaseMountableController mountable;
    public BaseMountableController Mountable => mountable;
}
