using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class UserActionSender<TActionCreator> : Singleton<UserActionSender<TActionCreator>> where TActionCreator : IActionCreator
{
    [SerializeField] protected List<StylizedTextImage> imageOverlay;

    private Dictionary<string, Dictionary<string, Dictionary<TActionCreator, UserAction>>> _actionCreators = new();

    protected abstract ESelectionType SelectionType();
    protected abstract ESubCategory SubCategory();

    private void Start()
    {
        SelectionManager.Instance.OnSelected += HandleSelection;
        SelectionManager.Instance.OnDeselected += HandleDeselection;
    }

    private void HandleSelection(IEnumerable<Selectable> selectables)
    {
        var categorizedActions = GetCategorizedActions(selectables);

        foreach (var category in categorizedActions)  
        {
            foreach (var labelGroup in category.Value)
            {
                UserActionManager.Instance.AddActionGroup(category.Key, SubCategory(), labelGroup.Key, labelGroup.Value);
            }
        }
    }

    private void HandleDeselection(IEnumerable<Selectable> selectables)
    {
        var categorizedActions = GetCategorizedActions(selectables);

        foreach (var category in categorizedActions)  
        {
            foreach (var labelGroup in category.Value)
            {
                UserActionManager.Instance.ClearActionGroup(category.Key, SubCategory(), labelGroup.Key, labelGroup.Value);
            }
        }
    }

    private Dictionary<string, Dictionary<string, List<UserAction>>> GetCategorizedActions(IEnumerable<Selectable> selectables)
    {
        Dictionary<string, Dictionary<string, List<UserAction>>> categorizedActions = new();
        
        foreach (var selectable in selectables.Where(s => s.SelectionType == SelectionType()))
        {
            if (!selectable.TryGetComponent<TActionCreator>(out var creator)) continue;
            
            var actions = creator.GetActions();

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

        return categorizedActions;
    }

    private void Update()
    {
        UpdateDisplayData();
    }

    private void UpdateDisplayData() //TODO: reference to actionCreator in user action, just call useraction.creator.updatedisplay(display)
                                     //from the useractiondisplay, then this can do whatever it wants to the display
    {
        foreach (var categoryKvp in _actionCreators)
        {
            foreach (var labelGroupKvp in categoryKvp.Value)
            {
                var display = UserActionManager.Instance.GetActionDisplay(categoryKvp.Key, SubCategory(), labelGroupKvp.Key);

                foreach (var actionCreatorKvp in labelGroupKvp.Value)
                {
                    UpdateDisplay(display, actionCreatorKvp.Key);
                }
            }
        }
    }

    protected abstract void UpdateDisplay(UserActionDisplay display, TActionCreator actionCreator);
}