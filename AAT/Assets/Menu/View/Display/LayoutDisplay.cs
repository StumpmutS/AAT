using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutDisplay : MonoBehaviour
{
    [SerializeField] private LayoutGroup layoutGroup;
    
    private HashSet<Transform> _children = new();

    private void Awake()
    {
        if (layoutGroup == null) layoutGroup = GetComponent<LayoutGroup>();
    }

    public void Add(Transform rectTransform)
    {
        _children.Add(rectTransform);
        rectTransform.SetParent(layoutGroup.transform, false);
    }

    public void Clear()
    {
        foreach (var child in _children)
        {
            if (child == null) continue;

            Destroy(child.gameObject);
        }
        
        _children.Clear();
    }
}