using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVisualsController : MonoBehaviour
{
    private GameObject _upgradesUI;
    private SelectableController _selectable;
    private SpawnerController _spawner;
    private List<UnitGroupController> _groups => _spawner.Groups;

    public event Action<SpawnerController> OnSelect = delegate { };
    public event Action<SpawnerController> OnDeselect = delegate { }; 

    private void Awake()
    {
        _selectable = GetComponent<SelectableController>();
        _selectable.OnSelect.AddListener(SelectAll);
        _selectable.OnDeselect.AddListener(DeselectAll);
    }

    private void Start()
    {
        _spawner = GetComponentInParent<SpawnerController>();
    }

    private void SelectAll()
    {
        OnSelect.Invoke(_spawner);
        foreach (var group in _groups)
        {
            group.SelectGroup();
        }
    }

    private void DeselectAll()
    {
        OnDeselect.Invoke(_spawner);
        foreach (var group in _groups)
        {
            group.DeselectGroup();
        }
    }

    private void OnDestroy()
    {
        _selectable.OnSelect.RemoveListener(SelectAll);
        _selectable.OnDeselect.RemoveListener(DeselectAll);
    }
}
