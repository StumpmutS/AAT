using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Utility.Scripts;

public class UnitMovementSystem : NetworkBehaviour, IMoveSystem
{
    //[Networked(OnChanged = nameof(OnCurrentSpeedChange))] private float CurrentSpeed { get; set; }
    public static void OnCurrentSpeedChange(Changed<MovementInteractComponentState> changed)
    {
        //todo just invoke event for animation/visuals listener ONLY PLACE WHERE SPEED CHANGE EVENT SHOULD BE NOW
        //changed.Behaviour._animation.SetMovement(changed.Behaviour.CurrentSpeed);
    }
     
    [SerializeField] private AgentBrain agentBrain;
    private IAgent _agent => agentBrain.CurrentAgent;

    public event Action OnPathFinished;

    public void Move(StumpTarget target)
    {
        throw new NotImplementedException();
    }

    public void Follow(StumpTarget target)
    {
        //set dest every frame?
    }

    public void Stop()
    {
        
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }
}
