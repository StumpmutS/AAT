using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[RequireComponent(typeof(NavMeshLink))]
public class PlaceableWallController : MonoBehaviour
{
    [SerializeField] private WallJointController wallJointPrefab;
    [SerializeField] private Vector3 wallJointOffset;
    [SerializeField] private WallVisualsController wallVisuals;
    public WallVisualsController WallVisuals => wallVisuals;

    protected List<WallJointController> _wallJoints = new List<WallJointController>();
    private NavMeshLink[] meshLinks;

    private void Awake()
    {
        meshLinks = GetComponents<NavMeshLink>();
    }

    public virtual void Setup(int connectedWallIndex, List<WallJointController> wallJoints = null)
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
        if (wallJoints != null) AddJointRange(wallJoints);
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
        AddJoint(instantiatedWallJoint);
    }

    private void AddJointRange(List<WallJointController> joints)
    {
        _wallJoints.AddRange(joints);
        foreach (var joint in joints) joint.AddWall(this);
    }

    private void AddJoint(WallJointController joint)
    {
        _wallJoints.Add(joint);
        joint.AddWall(this);
    }

    private void UpdateNavMeshLinks()
    {
        foreach (var meshLink in meshLinks)
        {
            meshLink.UpdateLink();
        }
    }
}
