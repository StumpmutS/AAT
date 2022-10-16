using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UserActionSender<TActionCreator> : Singleton<UserActionSender<TActionCreator>> where TActionCreator : IActionCreator
{
    [SerializeField] protected List<StylizedTextImage> imageOverlay;

    private Dictionary<string, Dictionary<string, List<TActionCreator>>> _actionCreators = new();

    protected abstract ESelectionType SelectionType();
    protected abstract ESubCategory SubCategory();

    private void Awake()
    {
        SelectionManager.Instance.OnSelected += HandleSelection;
    }

    private void HandleSelection(IEnumerable<SelectableController> selectables)
    {
        Dictionary<string, Dictionary<string, List<UserAction>>> categorizedActions = new();
        
        foreach (var selectable in selectables.Where(s => s.SelectionType == SelectionType()))
        {
            if (!selectable.TryGetComponent<TActionCreator>(out var container)) continue;
            
            var actions = container.GetActions();

            foreach (var action in actions)
            {
                if (!categorizedActions.ContainsKey(action.Category))
                {
                    categorizedActions[action.Category] = new Dictionary<string, List<UserAction>>();
                }

                if (!categorizedActions[action.Category].ContainsKey(action.Label))
                {
                    categorizedActions[action.Category][action.Label] = new List<UserAction>();
                }
                
                categorizedActions[action.Category][action.Label].Add(action);
            }
        }

        foreach (var category in categorizedActions)  
        {
            foreach (var labelGroup in category.Value)
            {
                UserActionManager.Instance.AddActionGroup(category.Key, SubCategory(), labelGroup.Key, labelGroup.Value);
            }
        }
    }

    private void Update()
    {
        UpdateDisplayData();
    }

    private void UpdateDisplayData()
    {
        foreach (var categoryKvp in _actionCreators)
        {
            foreach (var labelGroupKvp in categoryKvp.Value)
            {
                var display = UserActionManager.Instance.GetActionDisplay(categoryKvp.Key, SubCategory(), labelGroupKvp.Key);

                foreach (var container in labelGroupKvp.Value)
                {
                    UpdateDisplay(display, container);
                }
            }
        }
    }

    protected abstract void UpdateDisplay(UserActionDisplay display, TActionCreator abilityDataContainer);
}