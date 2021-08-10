using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DebugNavmeshSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().SetDestination(transform.position + (Vector3.forward * .01f));
    }
}
