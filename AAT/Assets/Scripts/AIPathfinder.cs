using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EntityController))]
public class AIPathfinder : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;

    private EntityController entity;

    private void Start()
    {
        entity = GetComponent<EntityController>();
        entity.OnSelect += SubscribeToInputManager;
        entity.OnDeselect += UnsubscribeFromInputManager;
    }

    private void SubscribeToInputManager()
    {
        InputController.OnRightClick += SetTargetDestination;
    }

    private void UnsubscribeFromInputManager()
    {
        InputController.OnRightClick -= SetTargetDestination;
    }

    private void SetTargetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
