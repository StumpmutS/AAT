using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UserActionDisplayContainer : MonoBehaviour
{
    [FormerlySerializedAs("layout")] [SerializeField] private LayoutDisplay categoryLayout;
    [FormerlySerializedAs("labeledLayoutPrefab")] [SerializeField] private LabeledLayoutDisplay categoryPrefab;
    [SerializeField] private LayoutDisplay subCategoryPrefab;
    [SerializeField] private UserActionDisplay userActionPrefab;

    private Dictionary<string, LabeledLayoutDisplay> _userActionCategories = new();
    private Dictionary<string, Dictionary<ESubCategory, LayoutDisplay>> _userActionSubCategories = new();
    private Dictionary<string, Dictionary<ESubCategory, Dictionary<string, UserActionDisplay>>> _userActionLabelGroups = new();

    public void Display(Dictionary<string, Dictionary<ESubCategory, Dictionary<string, List<UserAction>>>> userActions)
    {
        ClearDisplay();

        foreach (var categoryKvp in userActions)
        {
            var categoryDisplay = CreateCategoryDisplay(categoryKvp.Key);
            _userActionCategories[categoryKvp.Key] = categoryDisplay;
            
            foreach (var subCategoryKvp in categoryKvp.Value)
            {
                var subCategoryDisplay = CreateSubCategoryDisplay();
                categoryDisplay.Add(subCategoryDisplay.transform);
                _userActionSubCategories[categoryKvp.Key][subCategoryKvp.Key] = subCategoryDisplay;
                
                foreach (var labelGroupKvp in subCategoryKvp.Value)
                {
                    var userActionDisplay = CreateActionDisplay(labelGroupKvp.Value);
                    subCategoryDisplay.Add(userActionDisplay.transform);
                    _userActionLabelGroups[categoryKvp.Key][subCategoryKvp.Key][labelGroupKvp.Key] = userActionDisplay;
                }
            }
        }
    }

    private void ClearDisplay()
    {
        categoryLayout.Clear();
        _userActionCategories.Clear();
        _userActionSubCategories.Clear();
        _userActionLabelGroups.Clear();
    }

    private LabeledLayoutDisplay CreateCategoryDisplay(string category)
    {
        var categoryDisplay = Instantiate(categoryPrefab);
        categoryDisplay.SetText(category);
        categoryLayout.Add(categoryDisplay.transform);
        return categoryDisplay;
    }

    private LayoutDisplay CreateSubCategoryDisplay()
    {
        var subCategoryDisplay = Instantiate(subCategoryPrefab);
        return subCategoryDisplay;
    }

    private UserActionDisplay CreateActionDisplay(List<UserAction> userActions)
    {
        var actionDisplay = Instantiate(userActionPrefab);
        actionDisplay.Init(userActions);
        return actionDisplay;
    }

    public UserActionDisplay GetActionDisplay(string category, ESubCategory subCategory, string label)
    {
        return _userActionLabelGroups[category][subCategory][label];
    }

    public void ClearCategory(string category)
    {
        Destroy(_userActionCategories[category].gameObject);
    }
}