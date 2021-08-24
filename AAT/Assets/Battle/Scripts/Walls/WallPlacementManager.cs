using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class WallPlacementManager : MonoBehaviour
{
    [SerializeField] private float wallRotationAmount;
    [SerializeField] private Vector3 wallScaleAmount;
    [SerializeField] private NavMeshSurface ground;
    [SerializeField] private RectTransform wallPlacementUI;
    [SerializeField] private PlaceableWallController placeableWallPrefab;
    [SerializeField] private Vector3 wallOffset;
    [SerializeField] private LayerMask wallJointLayer;
    [SerializeField] private float snapRadius;

    public static event Action OnBeginWallPlacement = delegate { };
    public static event Action OnEndWallPlacement = delegate { };

    private Vector3 vectorWallRotationAmount;
    private Quaternion nextWallRotation;
    private bool wallPlacementActive = false;
    private WallVisualsController wallPreview;
    private Vector3 baseWallPreviewScale;

    private int connectedWallEndIndex = -1;

    private void Awake()
    {
        InputManager.OnTPressed += BeginWallPlacement;
        wallPreview = placeableWallPrefab.WallVisuals;
        vectorWallRotationAmount = new Vector3(0, wallRotationAmount, 0);
        nextWallRotation = Quaternion.Euler(Vector3.zero);
    }

    private void Start()
    {
        wallPreview = Instantiate(wallPreview);
        wallPreview.transform.localScale = placeableWallPrefab.transform.localScale;
        wallPreview.gameObject.SetActive(false);
        baseWallPreviewScale = wallPreview.transform.localScale;
    }

    public void BeginWallPlacement()
    {
        if (wallPlacementActive) return;
        wallPlacementActive = true;
        InputManager.OnTPressed += EndWallPlacement;
        InputManager.OnUpdate += MoveWallPreview;
        InputManager.OnMouseWheelScroll += RotateWallPreview;
        InputManager.OnPlus += ScaleUpWallPreview;
        InputManager.OnMinus += ScaleDownWallPreview;
        InputManager.OnLeftCLick += PlaceWall;
        OnBeginWallPlacement.Invoke();
        ActivateWallPreview();
        wallPlacementUI.gameObject.SetActive(true);
    }

    private void EndWallPlacement()
    {
        if (!wallPlacementActive) return;
        wallPlacementActive = false;
        InputManager.OnTPressed -= EndWallPlacement;
        InputManager.OnUpdate -= MoveWallPreview;
        InputManager.OnMouseWheelScroll -= RotateWallPreview;
        InputManager.OnPlus -= ScaleUpWallPreview;
        InputManager.OnMinus -= ScaleDownWallPreview;
        InputManager.OnLeftCLick -= PlaceWall;
        OnEndWallPlacement.Invoke();
        DeactivateWallPreview();
        wallPlacementUI.gameObject.SetActive(false);
    }

    #region WallPreview
    private void ActivateWallPreview()
    {
        wallPreview.gameObject.SetActive(true);
        MoveWallPreview();
    }

    private void DeactivateWallPreview()
    {
        wallPreview.gameObject.SetActive(false);
    }

    private void RotateWallPreview(float amount)
    {
        if (amount > 0)
        {
            nextWallRotation.eulerAngles += vectorWallRotationAmount;
            wallPreview.transform.rotation = nextWallRotation;
        }
        else
        {
            nextWallRotation.eulerAngles -= vectorWallRotationAmount;
            wallPreview.transform.rotation = nextWallRotation;
        }
    }

    private void ScaleUpWallPreview()
    {
        wallPreview.transform.localScale += wallScaleAmount;
    }

    private void ScaleDownWallPreview()
    {
        wallPreview.transform.localScale -= wallScaleAmount;
    }

    private void MoveWallPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            wallPreview.transform.position = hit.point + wallOffset;

            if (hit.collider.gameObject.GetComponent<NavMeshSurface>() != null)
            {
                CheckSnap();
            }
        }
    }

    private void CheckSnap()
    {
        int singleWallEndIndex = -1;
        List<WallJointController> joints = new List<WallJointController>();
        for (int i = 0; i < wallPreview.WallEnds.Count; i++)
        {
            if (CheckWallJoints(wallPreview.WallEnds[i], out var joint))
            {
                singleWallEndIndex = i;
                joints.Add(joint);
            }
        }
        if (joints.Count > 1)
        {
            connectedWallEndIndex = 2;
            Vector3 targetVector = joints[0].transform.position - joints[1].transform.position;

            wallPreview.transform.rotation = Quaternion.FromToRotation(Vector3.right, targetVector);

            wallPreview.transform.position = new Vector3(
                joints[1].transform.position.x + targetVector.x / 2, 
                joints[1].transform.position.y + targetVector.y / 2, 
                joints[1].transform.position.z + targetVector.z / 2);

            wallPreview.transform.localScale = new Vector3(
                targetVector.magnitude - (1 + Mathf.Abs(wallPreview.WallEnds[0].localPosition.x)),
                wallPreview.transform.localScale.y,
                wallPreview.transform.localScale.z);
        }
        else if (joints.Count > 0)
        {
            connectedWallEndIndex = singleWallEndIndex;
            Vector3 endOffset = wallPreview.transform.position - wallPreview.WallEnds[singleWallEndIndex].position;
            wallPreview.transform.position = joints[0].transform.position + endOffset;
        }
        else
        {
            connectedWallEndIndex = -1;
        }
    }

    private bool CheckWallJoints(Transform wallEnd, out WallJointController wallJoint)
    {
        wallJoint = null;
        float endDistanceSqr = Mathf.Infinity;

        Collider[] snapColliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(wallEnd.position, snapRadius, snapColliders, wallJointLayer);
        foreach (var collider in snapColliders)
        {
            if (collider == null) continue;
            float newDistanceSqr = (wallEnd.position - collider.transform.position).sqrMagnitude;
            if (newDistanceSqr < endDistanceSqr)
            {
                endDistanceSqr = newDistanceSqr;
                wallJoint = collider.GetComponent<WallJointController>();
            }
        }
        if (wallJoint == null) return false;
        return true;
    }
    #endregion

    private void PlaceWall()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            if (hit.collider.gameObject.GetComponent<NavMeshSurface>() != null)
            {
                var instantiatedWall = Instantiate(placeableWallPrefab, wallPreview.transform.position, wallPreview.transform.rotation);
                instantiatedWall.transform.localScale = wallPreview.transform.localScale;
                wallPreview.transform.localScale = baseWallPreviewScale;
                if (connectedWallEndIndex == 1)
                {
                    instantiatedWall.Setup(0);
                }
                else if (connectedWallEndIndex == 0)
                {
                    instantiatedWall.Setup(1);
                }
                else
                {
                    instantiatedWall.Setup(connectedWallEndIndex);
                }
            }
        }
    }
}
