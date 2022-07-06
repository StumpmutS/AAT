using UnityEngine;

public class WallJointConnectorController : MonoBehaviour
{
    [SerializeField] private DimensionsContainer dimensionsContainer;
    public DimensionsContainer DimensionsContainer => dimensionsContainer;
    [SerializeField] private BasePreviewPoolingObject connectorVisuals;
    public BasePreviewPoolingObject ConnectorVisuals => connectorVisuals;
}
