using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SelectableController))]
public class WallJointController : MonoBehaviour
{
    [SerializeField] private WallPlacementManager wallPlacementManager;
    [SerializeField] private WallJointConnectorController connectorPrefab;
    public WallJointConnectorController ConnectorPrefab => connectorPrefab;
    [SerializeField] private PreviewPoolingObject jointPreview;
    public PreviewPoolingObject JointPreview => jointPreview;
    [SerializeField] private DimensionsContainer dimensionsContainer;
    public DimensionsContainer DimensionsContainer => dimensionsContainer;
    [SerializeField] private NavMeshObstacle obstacle;
    public NavMeshObstacle Obstacle => obstacle;
    public float ConnectorXDimensions => connectorPrefab.DimensionsContainer.XDimensions;

    public int WallNumber { get; private set; }
    
    private SelectableController _selectable;
    private Vector3 _wallDirection;
    private Vector3 _wallForwardDirection;

    public event Action<WallJointController> OnJointSelect = delegate { };

    private void Awake()
    {
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect += HandleSelect;
        if (wallPlacementManager == null) wallPlacementManager = WallPlacementManager.Instance;
        wallPlacementManager.AddWallJoint(this);
    }

    public PreviewPoolingObject SetupConnectorPreview(Vector3 normalizedDirection, bool reverse, float spaceToFill)
    {
        //TODO: spacetofill is for outer part of wall connector
        if (spaceToFill < 0) spaceToFill = ConnectorXDimensions;
        var connectorPreview = PoolingManager.Instance.CreatePoolingObject(connectorPrefab.ConnectorVisuals) as PreviewPoolingObject;
        connectorPreview.transform.right = normalizedDirection;
        if (Vector3.Dot(connectorPreview.transform.forward, transform.forward) < 0)
        {
            connectorPreview.transform.right = -normalizedDirection;
        }
        if (reverse) connectorPreview.transform.right = -connectorPreview.transform.right;
        var newConnectorScale = spaceToFill / ConnectorXDimensions + 1;
        connectorPreview.transform.position = transform.position + normalizedDirection * newConnectorScale * ConnectorXDimensions / 2;
        connectorPreview.transform.localScale = new Vector3(newConnectorScale,
            connectorPreview.transform.localScale.y, connectorPreview.transform.localScale.z);
        return connectorPreview;
    }

    public void SetupConnector(WallJointConnectorController connector, Vector3 direction)
    {
        _wallForwardDirection = connector.transform.forward;
        connector.transform.parent = transform;
        _wallDirection = direction;
        WallNumber++;
    }

    public bool AngleValid(Vector3 direction, float minAngle)
    {
        if (_wallDirection == default) return true;
        return !(Vector3.Angle(_wallDirection, direction) < minAngle);
    }

    public bool CheckChain(Vector3 direction, Vector3 forwardDirection)
    {
        if (WallNumber < 1) return false;
        var currentDotPotential = Vector3.Dot(_wallDirection, direction);
        if (currentDotPotential > 0)
        {
            return Vector3.Dot(forwardDirection, _wallForwardDirection) < 0;
        }
        return Vector3.Dot(forwardDirection, _wallForwardDirection) > 0;
    }
    
    private void HandleSelect()
    {
        OnJointSelect.Invoke(this);
    }
}
