using UnityEngine;

public class PlaceableWallController : WallController
{
    [SerializeField] private PreviewPoolingObject wallVisuals;
    public PreviewPoolingObject WallVisuals => wallVisuals;
    [SerializeField] private BaseMountableController mountable;
    public BaseMountableController Mountable => mountable;
}
