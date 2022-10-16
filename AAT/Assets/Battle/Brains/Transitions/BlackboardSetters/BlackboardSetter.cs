using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public abstract class BlackboardSetter<T> : NetworkBehaviour where T : TransitionBlackboard
{
    [SerializeField] private Brain<T> brain;

    protected T _blackboard => brain.GetBlackboard();
}

public class BlackboardAgentReadySetter : BlackboardSetter<AiTransitionBlackboard>
{
    
}

public class BlackboardPatrolSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private PatrolDefaults patrolDefaults;

    private void SetPatrolState(object thing)
    {
        
    }
}

public class PatrolDefaults : ScriptableObject
{
    [SerializeField] private List<StylizedTextImage> icon;
    public List<StylizedTextImage> Icon => icon;
    [SerializeField] private string label;
    public string Label => label;
    [SerializeField] private KeyCode keyCode;
    public KeyCode KeyCode => keyCode;
    [SerializeField] private ESelectionType category;
    public ESelectionType Category => category;
}

public class PatrolButtonSender : UserActionSender<MovementDataContainer>
{
    protected override ESelectionType SelectionType() => ESelectionType.Group;

    protected override ESubCategory SubCategory() => ESubCategory.Movement;

    protected override void UpdateDisplay(UserActionDisplay display, MovementDataContainer abilityDataContainer)
    {
        throw new NotImplementedException();
    }
}

public class MovementDataContainer : MonoBehaviour, IActionCreator
{
    public List<UserAction> GetActions()
    {
        throw new NotImplementedException();
    }
}