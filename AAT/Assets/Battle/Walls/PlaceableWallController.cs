using UnityEngine;

public class PlaceableWallController : WallController
{
    [SerializeField] private BasePreviewPoolingObject wallVisuals;
    public BasePreviewPoolingObject WallVisuals => wallVisuals;
    [SerializeField] private BaseMountableController mountable;
    public BaseMountableController Mountable => mountable;
}
