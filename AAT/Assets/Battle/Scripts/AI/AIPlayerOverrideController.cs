using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class AIPlayerOverrideController : AIPathfinder
{
    private EntityController entity;

    private void Awake()
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

    private void SetTargetDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit))
        {
            Vector3 destination = hit.point;
            agent.SetDestination(destination);
        }
    }
}