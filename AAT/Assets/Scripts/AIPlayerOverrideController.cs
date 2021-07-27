using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class AIPlayerOverrideController : AIPathfinder
{
    private EntityController entity;

    private void Start()
    {
        entity = GetComponent<EntityController>();
        entity.OnSelect += SubscribeToInputManager;
        entity.OnDeselect += UnsubscribeFromInputManager;
    }

    private void SubscribeToInputManager()
    {
        InputManager.OnRightClick += SetTargetDestination;
    }

    private void UnsubscribeFromInputManager()
    {
        InputManager.OnRightClick -= SetTargetDestination;
    }

    private void SetTargetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
