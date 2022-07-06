using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class NetworkedPreviewPoolingObject : BasePreviewPoolingObject
{
    [SerializeField] private NetworkPrefabRef actualPrefab;
    
    protected override GameObject CreateObject()
    {
        return StumpNetworkRunner.Instance.Runner.Spawn(actualPrefab, transform.position, transform.rotation).gameObject;
    }
}
