using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NetworkedButton : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnChanged))] private NetworkBool networkClick { get; set; }
    public static void OnChanged(Changed<NetworkedButton> changed)
    {
        if (changed.Behaviour.networkClick) changed.Behaviour.Click();
    }
    
    [SerializeField] private Button button;

    public UnityEvent OnClick;

    private bool _localClicked;

    private void Awake()
    {
        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        _localClicked = true;
    }

    public override void FixedUpdateNetwork()
    {
        if (_localClicked != networkClick) networkClick = _localClicked;
        _localClicked = false;
    }

    private void Click()
    {
        OnClick.Invoke();
    }
}
