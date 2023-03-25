using UnityEngine;

public class BlackboardPatrolSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private PatrolDefaults patrolDefaults;

    private void SetPatrolState(object thing)
    {
        //_blackboard.PatrolReady = true; TODO
    }
}