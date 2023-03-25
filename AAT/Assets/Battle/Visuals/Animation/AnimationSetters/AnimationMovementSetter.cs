using System;
using UnityEngine;

public class AnimationMovementSetter : MonoBehaviour
{
    [SerializeField] private AgentBrain agentBrain;
    [SerializeField] private StumpAnimationController animationController;

    private void Start()
    {
        agentBrain.OnPathSet += HandlePathChanged;
        agentBrain.OnPathFinished += HandlePathChanged;
        HandlePathChanged();
    }

    private void HandlePathChanged()
    {
        animationController.SetMovement(agentBrain.GetSpeed());
    }

    private void OnDestroy()
    {
        if (agentBrain != null) agentBrain.OnPathSet += HandlePathChanged;
        if (agentBrain != null) agentBrain.OnPathFinished += HandlePathChanged;
    }
}