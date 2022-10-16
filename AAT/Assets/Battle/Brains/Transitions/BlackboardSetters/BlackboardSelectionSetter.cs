using UnityEngine;

public class BlackboardSelectionSetter : BlackboardSetter<AiTransitionBlackboard>
{
    [SerializeField] private SelectableController selectable;

    private void Awake()
    {
        selectable.OnSelect.AddListener(HandleSelect);
        selectable.OnDeselect.AddListener(HandleDeselect);
    }

    private void HandleSelect()
    {
        _blackboard.Selected = true;
    }

    private void HandleDeselect()
    {
        _blackboard.Selected = false;
    }
}