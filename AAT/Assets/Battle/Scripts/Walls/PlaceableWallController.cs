using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshLink))]
public class PlaceableWallController : MonoBehaviour
{
    [SerializeField] private GameObject wallJointPrefab;
    [SerializeField] private Vector3 wallJointOffset;
    [SerializeField] private WallVisualsController wallVisuals;
    public WallVisualsController WallVisuals => wallVisuals;


    private NavMeshLink[] meshLinks;

    private void Awake()
    {
        meshLinks = GetComponents<NavMeshLink>();
    }

    public void Setup(int connectedWallIndex)
    {
        if (connectedWallIndex < 0)
        {
            for (int i = 0; i < wallVisuals.WallEnds.Count; i++)
            {
                SpawnWall(i);
            }
        }
        else if (connectedWallIndex < 2)
        {
            SpawnWall(connectedWallIndex);
        }    
        UpdateNavMeshLinks();
    }

    private void SpawnWall(int wallEndIndex)
    {
        var instantiatedWallJoint = Instantiate(wallJointPrefab, wallVisuals.WallEnds[wallEndIndex]);
        instantiatedWallJoint.transform.position += wallJointOffset;
        instantiatedWallJoint.transform.localScale = new Vector3(
            instantiatedWallJoint.transform.localScale.x / gameObject.transform.localScale.x,
            instantiatedWallJoint.transform.localScale.y / gameObject.transform.localScale.y,
            instantiatedWallJoint.transform.localScale.z / gameObject.transform.localScale.z);
    }

    private void UpdateNavMeshLinks()
    {
        foreach (var meshLink in meshLinks)
        {
            meshLink.UpdateLink();
        }
    }
}
