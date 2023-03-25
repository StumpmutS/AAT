using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UserActionDisplay : MonoBehaviour
{
    [SerializeField] private FullImageTextProperty iconProperty;

    public void Init(List<UserAction> userActions)
    {
        var actionSample = userActions.First();
        iconProperty.Init(actionSample.Icon, actionSample.Label, HandleInteraction, userActions);
    }

    private void HandleInteraction(object obj)
    {
        if (obj is not List<UserAction> userActions) return;
        
        foreach (var userAction in userActions)
        {
            userAction.SelectedAction(userAction.ActionObj);
        }
    }
}