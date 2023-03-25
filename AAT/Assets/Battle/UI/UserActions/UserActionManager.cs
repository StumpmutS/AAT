using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserActionManager : MonoBehaviour
{
    [SerializeField] private UserActionDisplayContainer displayContainer;

    public static UserActionManager Instance { get; private set; }
    
    private Dictionary<string, Dictionary<ESubCategory, Dictionary<string, List<UserAction>>>> _userActions = new();
    private bool _dirty;
    
    /*
    EXAMPLE:
     
    string Category: Wolf Unit Group 
    {
        ESubCategory SubCategory: Ability
        {
            string Label: Howl
            {
                User Action
            }
            
            string Label: WolfAbilityName2
            {
                User Action
            }
        }
        
        ESubCategory SubCategory: Movement
        {
            string Label: Patrol
            {
                User Action
            }
            
            string Label: Attack Move
            {
                User Action
            }
        }
    }
    
    string Category: Bear Spawner...
    */

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    private void Update()
    {
        if (_dirty)
        {
            displayContainer.Display(_userActions);
        }

        _dirty = false;
    }

    public void AddActionGroup(string category, ESubCategory subCategory, string label, List<UserAction> actions)
    {
        if (!_userActions.ContainsKey(category)) _userActions[category] = new Dictionary<ESubCategory, Dictionary<string, List<UserAction>>>();
        if (!_userActions[category].ContainsKey(subCategory)) _userActions[category][subCategory] = new Dictionary<string, List<UserAction>>();
        if (!_userActions[category][subCategory].ContainsKey(label)) _userActions[category][subCategory][label] = new List<UserAction>();

        _userActions[category][subCategory][label].AddRange(actions);
        _dirty = true;
    }

    public UserActionDisplay GetActionDisplay(string category, ESubCategory subCategory, string label)
    {
        return displayContainer.GetActionDisplay(category, subCategory, label);
    }

    public void ClearActionGroup(string category, ESubCategory subCategory, string label, List<UserAction> actions)
    {
        if (_userActions.TryGetValue(category, out var categoryDictionary))
        {
            if (categoryDictionary.TryGetValue(subCategory, out var subCategoryDictionary))
            {
                if (subCategoryDictionary.TryGetValue(label, out var activeActions))
                {
                    foreach (var action in actions)
                    {
                        activeActions.Remove(action);
                    }

                    if (activeActions.Count < 1)
                    {
                        _userActions[category][subCategory].Remove(label);
                        if (_userActions[category][subCategory].Count < 1)
                        {
                            _userActions[category].Remove(subCategory);
                            if (_userActions[category].Count < 1)
                            {
                                _userActions.Remove(category);
                            }
                        }
                    }
                }
            }
        }
        
        _dirty = true;
    }
}
