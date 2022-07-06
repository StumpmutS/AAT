using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SelectableController))]
public class WallJointController : MonoBehaviour
{
    [Tooltip("Must be < 30")] [SerializeField] private float maxMainWallValidAngle;
    [SerializeField] private WallPlacementManager wallPlacementManager;
    [SerializeField] private WallJointConnectorController connectorPrefab;
    public WallJointConnectorController ConnectorPrefab => connectorPrefab;
    [SerializeField] private BasePreviewPoolingObject jointPreview;
    public BasePreviewPoolingObject JointPreview => jointPreview;
    [SerializeField] private DimensionsContainer dimensionsContainer;
    public DimensionsContainer DimensionsContainer => dimensionsContainer;
    [SerializeField] private NavMeshObstacle obstacle;
    public NavMeshObstacle Obstacle => obstacle;
    public float ConnectorXDimensions => connectorPrefab.DimensionsContainer.XDimensions;

    public int WallNumber { get; private set; }
    
    private SelectableController _selectable;
    private Vector3 _wallDirection;
    private Vector3 _wallForwardDirection;

    [HideInInspector][SerializeField] private List<Vector3> _mainWallDirections;

    public event Action<WallJointController> OnJointSelect = delegate { };

    private void Awake()
    {
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect += HandleSelect;
        if (wallPlacementManager == null) wallPlacementManager = WallPlacementManager.Instance;
        wallPlacementManager.AddWallJoint(this);
    }

    public void SetManager(WallPlacementManager manager)
    {
        wallPlacementManager = manager;
    }

    public BasePreviewPoolingObject SetupConnectorPreview(Vector3 normalizedDirection, bool reverse, float spaceToFill)
    {
        if (spaceToFill < 0) spaceToFill = ConnectorXDimensions;
        var connectorPreview = PoolingManager.Instance.CreatePoolingObject(connectorPrefab.ConnectorVisuals) as BasePreviewPoolingObject;
        connectorPreview.transform.right = normalizedDirection;
        if (Vector3.Dot(connectorPreview.transform.forward, transform.forward) < 0)
        {
            connectorPreview.transform.right = -normalizedDirection;
        }
        if (reverse) connectorPreview.transform.right = -connectorPreview.transform.right;
        var newConnectorScale = spaceToFill / ConnectorXDimensions + 1;
        connectorPreview.transform.position = transform.position + normalizedDirection * (newConnectorScale * ConnectorXDimensions / 2);
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

    public void SetupMainConnector(WallJointConnectorController connector, Vector3 direction)
    {
        _mainWallDirections ??= new List<Vector3>();
        _mainWallDirections.Add(direction);
        connector.transform.parent = transform;
    }

    public bool AngleValid(Vector3 direction, float minAngle)
    {
        if (_mainWallDirections.Any(dir => Vector3.Angle(dir, direction) <= maxMainWallValidAngle)) return false;
        if (_wallDirection == default) return true;
        return Vector3.Angle(_wallDirection, direction) >= minAngle;
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
