using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserActionManager : MonoBehaviour
{
    [SerializeField] private UserActionDisplayContainer displayContainer;

    public static UserActionManager Instance { get; private set; }
    
    private Dictionary<string, Dictionary<ESubCategory, Dictionary<string, List<UserAction>>>> _userActions = new();
    
    /*
    string Category: Wolf Unit Group 
    {
        ESubCategory SubCategory: Ability
        {
            string Label: Howl
            {
                User Actions
            }
            
            string Label: WolfAbilityName2
            {
                User Actions
            }
        }
        
        ESubCategory SubCategory: Movement
        {
            string Label: Patrol
            {
                User Actions
            }
            
            string Label: Attack Move
            {
                User Actions
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

    public void AddActionGroup(string category, ESubCategory subCategory, string label, List<UserAction> actions)
    {
        _userActions[category][subCategory][label].AddRange(actions);
        displayContainer.Display(_userActions);
    }

    public UserActionDisplay GetActionDisplay(string category, ESubCategory subCategory, string label)
    {
        return displayContainer.GetActionDisplay(category, subCategory, label);
    }

    public void ClearCategory(string category)
    {
        displayContainer.ClearCategory(category);
    }
}
