using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ExpandableUIGroup : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private RectTransform ownGroupContainer;
    [Tooltip("Start with the bottom-most groupContainer at index 0 and work way up")]
    [SerializeField] private List<RectTransform> containersAscending;
    [SerializeField] private RectTransform highestLayoutParent;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(HandleToggle);
    }

    private void Start()
    {
        AlterGroup(false);
    }

    private void HandleToggle(bool value)
    {
        AlterGroup(!value);
    }

    private void AlterGroup(bool value)
    {
        ownGroupContainer.gameObject.SetActive(value);
        foreach (var container in containersAscending)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(highestLayoutParent);
    }
}
