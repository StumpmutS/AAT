using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Utility.Scripts;

public class WallPlacementManager : MonoBehaviour
{
    [SerializeField] private RectTransform wallPlacementUI;
    [SerializeField] private MountablePointLinkController mountablePointLinkPrefab;
    [SerializeField] private PlaceableWallController placeableWallPrefab;
    [SerializeField] private Vector3 wallOffset;
    [SerializeField] private LayerMask wallJointLayer;
    [SerializeField] [Tooltip("Must be greater than min distance ((Wall Dim+Joint Dim)*2+.01)")] private float snapRadius;
    [SerializeField] private float wallMinValidAngle;

    public static WallPlacementManager Instance { get; private set; }
    
    private HashSet<WallJointController> _wallJoints = new();
    private WallJointController _currentWallJoint;
    private Vector3 _currentJointPositionZeroY;
    private Collider _currentJointCollider;
    private bool _wallPlacementActive;
    private bool _validPlacement;
    
    private void Awake()
    {
        Instance = this;
        InputManager.OnTPressed += ActivateWallPlacementInterface;
    }

    public void AddWallJoint(WallJointController wallJoint)
    {
        _wallJoints.Add(wallJoint);
        wallJoint.OnJointSelect += BeginWallPlacement;
    }

    private void ActivateWallPlacementInterface()
    {
        _wallPlacementActive = true;
        InputManager.OnTPressed -= ActivateWallPlacementInterface;
        InputManager.OnTPressed += DeactivateWallPlacementInterface;
    }
    
    private void DeactivateWallPlacementInterface()
    {
        _wallPlacementActive = false;
        InputManager.OnTPressed -= DeactivateWallPlacementInterface;
        InputManager.OnTPressed += ActivateWallPlacementInterface;
    }

    private void BeginWallPlacement(WallJointController wallJoint)
    {
        if (_currentWallJoint != null || !_wallPlacementActive || wallJoint.WallNumber > 1) return;
        _currentWallJoint = wallJoint;
        _currentJointPositionZeroY = new Vector3(_currentWallJoint.transform.position.x, 0,
            _currentWallJoint.transform.position.z);
        _currentJointCollider = wallJoint.GetComponent<Collider>();
        InputManager.OnRightClickDown += EndWallPlacement;
        InputManager.OnUpdate += MoveWallPreview;
        InputManager.OnLeftCLickUp += PlaceWall;
        InputManager.OnRPressed += ReversePreviewDirection;
        wallPlacementUI.gameObject.SetActive(true);
    }

    private void EndWallPlacement()
    {
        if (_currentWallJoint == null) return;
        _currentWallJoint = null;
        foreach (var preview in _wallPreviews)
        {
            preview.Deactivate();
        }
        _currentConnectorPreview.Deactivate();
        if (_createdConnectorPreview != null) _createdConnectorPreview.Deactivate();
        if (_createdJointPreview != null) _createdJointPreview.Deactivate();
        InputManager.OnRightClickDown -= EndWallPlacement;
        InputManager.OnUpdate -= MoveWallPreview;
        InputManager.OnLeftCLickUp -= PlaceWall;
        InputManager.OnRPressed -= ReversePreviewDirection;
        wallPlacementUI.gameObject.SetActive(false);
    }

    #region WallPreview
    private bool _reverse;
    private HashSet<PreviewPoolingObject> _wallPreviews = new();
    private PreviewPoolingObject _currentConnectorPreview;
    private PreviewPoolingObject _createdConnectorPreview;
    private PreviewPoolingObject _createdJointPreview;
    private WallJointController _snapJoint;
    private Vector3 _normDirection;

    private void MoveWallPreview()
    {
        var point = Vector3.zero;
        if (point.SetToCursorToWorldPosition(LayerManager.Instance.GroundLayer))
        {
            CreateWallPreviews(point);
        }
    }

    private void ReversePreviewDirection() => _reverse = !_reverse;

    private void CreateWallPreviews(Vector3 point)
    {
        //SNAP
        var snap = false;
        var colliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(point, snapRadius, colliders, wallJointLayer);
        var minDistance = (placeableWallPrefab.DimensionsContainer.XDimensions + _currentWallJoint.ConnectorXDimensions) * 2 + .01f;
        var ignoreColliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(_currentWallJoint.transform.position, minDistance, ignoreColliders, wallJointLayer);
        for (var i = 0; i < 10; i++)
        {
            if (ignoreColliders.Contains(colliders[i]))
            {
                colliders[i] = null;
                continue;
            }
            if (colliders[i] == null) continue;
            if (!colliders[i].GetComponent<WallJointController>()
                .AngleValid(_currentWallJoint.transform.position - colliders[i].transform.position, 60))
            {
                colliders[i] = null;
            }
        }
        var closest = DistanceCompare.FindClosestThing(colliders, point, ignore:_currentJointCollider);
        if (closest != null)
        {
            point = closest.transform.position;
            _snapJoint = closest.GetComponent<WallJointController>();
            snap = true;
        }
        
        //CRUNCH THE NUMBERS
        DeactivatePreviews();
        _wallPreviews.Clear();
        var dragDirection =  new Vector3(point.x, 0, point.z) - _currentJointPositionZeroY;
        _normDirection = dragDirection.normalized;
        if (minDistance > dragDirection.magnitude)
        {
            dragDirection = _normDirection * minDistance;
            point = new Vector3(_currentJointPositionZeroY.x + dragDirection.x, point.y, _currentJointPositionZeroY.z + dragDirection.z);
        }
        var exactWallNumber = (dragDirection.magnitude - _currentWallJoint.ConnectorXDimensions * 2) /
                             placeableWallPrefab.DimensionsContainer.XDimensions;
        var wallNumber = Mathf.Floor(exactWallNumber);
        var scaleFillAmount = (exactWallNumber - wallNumber) * placeableWallPrefab.DimensionsContainer.XDimensions / 2;
        var connectorSize = scaleFillAmount / _currentWallJoint.ConnectorXDimensions + 1;
        var connectorOffset = (connectorSize * _currentWallJoint.ConnectorXDimensions + placeableWallPrefab.DimensionsContainer.XDimensions / 2) * _normDirection;
        var jointConnectOffset = _currentJointPositionZeroY + wallOffset + connectorOffset;

        //SPAWN THE STUFF
        for (int i = 0; i < wallNumber; i++)
        {
            var wallVisuals = PoolingManager.Instance.CreatePoolingObject(placeableWallPrefab.WallVisuals) as PreviewPoolingObject;
            wallVisuals.transform.position = _normDirection * (i * placeableWallPrefab.DimensionsContainer.XDimensions) + jointConnectOffset;
            wallVisuals.transform.right = dragDirection;
            if (Vector3.Dot(wallVisuals.transform.forward, _currentWallJoint.transform.forward) < 0)
                wallVisuals.transform.right = -dragDirection;
            if (_reverse) wallVisuals.transform.forward = -wallVisuals.transform.forward;
            wallVisuals.gameObject.SetActive(true);
            _wallPreviews.Add(wallVisuals);
        }
        var currentWallConnector = _currentWallJoint.SetupConnectorPreview(_normDirection, _reverse, scaleFillAmount);
        _currentConnectorPreview = currentWallConnector;
        var createdJointPreview = PoolingManager.Instance.CreatePoolingObject(_currentWallJoint.JointPreview) as PreviewPoolingObject;
        createdJointPreview.transform.rotation = _currentWallJoint.transform.rotation;
        createdJointPreview.transform.position = new Vector3(point.x, wallOffset.y, point.z);
        var createdConnectorPreview = PoolingManager.Instance.CreatePoolingObject(_currentWallJoint.ConnectorPrefab.ConnectorVisuals) as PreviewPoolingObject;
        createdConnectorPreview.transform.position = createdJointPreview.transform.position - _normDirection * (connectorSize * _currentWallJoint.ConnectorXDimensions / 2);
        createdConnectorPreview.transform.rotation = currentWallConnector.transform.rotation;
        createdConnectorPreview.transform.localScale = currentWallConnector.transform.localScale;
        _createdJointPreview = createdJointPreview;
        _createdConnectorPreview = createdConnectorPreview;
        if (snap)
        {
            _createdJointPreview.Deactivate();
            _createdJointPreview = null;
        }
        
        //VALID CHECKS
        var obstacleSize = _normDirection * (_currentWallJoint.DimensionsContainer.XDimensions * _currentWallJoint.transform.localScale.x * _currentWallJoint.Obstacle.radius);
        if (NavMesh.SamplePosition(_currentJointPositionZeroY + obstacleSize, out var hit, 6, -1))
        {
            var target = snap ? point - obstacleSize : point;
            if (minDistance > (target - _currentJointPositionZeroY).magnitude) target = _currentJointPositionZeroY + _normDirection * minDistance;
            if (NavMesh.Raycast(hit.position, target, out var notNeeded, -1))
            {
                SetInvalid();
                return;
            }
        }
        else
        {
            Debug.LogError("No position found on NavMesh to sample");
        }
        if (!_currentWallJoint.AngleValid(_normDirection, wallMinValidAngle))
        {
            SetInvalid();
            return;
        }
        SetValid();
    }

    private void DeactivatePreviews()
    {
        foreach (var wallVisualsObject in _wallPreviews)
        {
            wallVisualsObject.Deactivate();
        }
        if (_createdConnectorPreview != null) _createdConnectorPreview.Deactivate();
        if (_currentConnectorPreview != null) _currentConnectorPreview.Deactivate();
        if (_createdJointPreview != null) _createdJointPreview.Deactivate();
    }

    private void SetValid()
    {
        _validPlacement = true;
        foreach (var previewWall in _wallPreviews)
        {
            previewWall.Preview.SetValid();
        }
        if (_createdConnectorPreview != null) _createdConnectorPreview.Preview.SetValid();
        if (_currentConnectorPreview != null) _currentConnectorPreview.Preview.SetValid();
        if (_createdJointPreview != null) _createdJointPreview.Preview.SetValid();
    }

    private void SetInvalid()
    {
        _validPlacement = false;
        foreach (var previewWall in _wallPreviews)
        {
            previewWall.Preview.SetInvalid();
        }
        if (_createdConnectorPreview != null) _createdConnectorPreview.Preview.SetInvalid();
        if (_currentConnectorPreview != null) _currentConnectorPreview.Preview.SetInvalid();
        if (_createdJointPreview != null) _createdJointPreview.Preview.SetInvalid();
    }
    #endregion

    private void PlaceWall()
    {
        if (!_validPlacement) return;
        
        var currentChain = CheckChain(_currentWallJoint, _normDirection);
        var otherChain = false;
        if (_snapJoint != null) otherChain = CheckChain(_snapJoint, -_normDirection);
        var wallMountables = _wallPreviews.Select(wallPreview => wallPreview.InitiateUsage().GetComponent<PlaceableWallController>().Mountable).ToList();

        _currentWallJoint.SetupConnector(_currentConnectorPreview.InitiateUsage().GetComponent<WallJointConnectorController>(), _normDirection);
        _currentConnectorPreview.Deactivate();
        
        var endJoint = _createdJointPreview == null ? _snapJoint : _createdJointPreview.InitiateUsage().GetComponent<WallJointController>();
        endJoint.SetupConnector(_createdConnectorPreview.InitiateUsage().GetComponent<WallJointConnectorController>(), -_normDirection);
        _createdConnectorPreview.Deactivate();
        if (_createdJointPreview != null) _createdJointPreview.Deactivate();
            
        SetupMountables(endJoint, wallMountables, currentChain, otherChain);
        EndWallPlacement();
    }
    
    private bool CheckChain(WallJointController joint, Vector3 direction)
    {
        foreach (var wall in _wallPreviews)
        {
            return joint.CheckChain(direction, wall.transform.forward);
        }
        Debug.LogError("Wall collection was empty");
        return false;
    }

    private void SetupMountables(WallJointController endJoint, List<BaseMountableController> mountables, bool currentChain, bool otherChain)
    {
        var currentPoint = _currentWallJoint.GetComponent<LinkPointController>();
        var endPoint = endJoint.GetComponent<LinkPointController>();
        
        var instantiatedLink = Instantiate(mountablePointLinkPrefab);
        if (currentChain)
        {
            if (otherChain)
            {        
                if (currentPoint.Start && !endPoint.Start)
                {
                    mountables.Reverse();
                    instantiatedLink.CreateLinkDouble(endPoint, currentPoint, mountables);
                }
                else instantiatedLink.CreateLinkDouble(currentPoint, endPoint, mountables);
                return;
            }
            print("current");
            instantiatedLink.CreateLinkSingle(_currentWallJoint.GetComponent<LinkPointController>(), endJoint.GetComponent<LinkPointController>(), mountables);
            return;
        }
        if (otherChain)
        {
            print("other");
            instantiatedLink.CreateLinkSingle(endJoint.GetComponent<LinkPointController>(), _currentWallJoint.GetComponent<LinkPointController>(), mountables, true);
            return;
        }
        
        instantiatedLink.CreateNewLink(_currentWallJoint.GetComponent<LinkPointController>(), endJoint.GetComponent<LinkPointController>(), mountables);
    }
}
