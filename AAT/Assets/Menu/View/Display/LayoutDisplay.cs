using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class LayoutDisplay : MonoBehaviour
{
    private HashSet<Transform> _children = new();
    private LayoutGroup _layoutGroup;

    private void Awake()
    {
        _layoutGroup = GetComponent<LayoutGroup>();
    }

    public void Add(Transform rectTransform)
    {
        _children.Add(rectTransform);
        rectTransform.SetParent(_layoutGroup.transform, false);
    }

    public void Clear()
    {
        foreach (var child in _children)
        {
            Destroy(child.gameObject);
        }
        
        _children.Clear();
    }
}