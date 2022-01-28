using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJointConnectorController : MonoBehaviour
{
    [SerializeField] private DimensionsContainer dimensionsContainer;
    public DimensionsContainer DimensionsContainer => dimensionsContainer;
    [SerializeField] private PreviewPoolingObject connectorVisuals;
    public PreviewPoolingObject ConnectorVisuals => connectorVisuals;
}
