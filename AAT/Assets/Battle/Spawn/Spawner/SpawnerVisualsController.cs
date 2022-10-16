using System;
using UnityEngine;

public class SpawnerVisualsController : MonoBehaviour
{
    [SerializeField] private SelectableController selectable;
    [SerializeField] private GroupSpawnerController spawner;
    
    private GameObject _upgradesUI;
    //private List<UnitGroupController> _groups => _spawner.Groups;

    public event Action<GroupSpawnerController> OnSelect = delegate { };
    public event Action<GroupSpawnerController> OnDeselect = delegate { }; 

    private void Awake()
    {
        selectable.OnSelect.AddListener(SelectAll);
        selectable.OnDeselect.AddListener(DeselectAll);
    }

    private void SelectAll()
    {
        OnSelect.Invoke(spawner);
        throw new NotImplementedException();
        /*foreach (var group in _groups)
        {
            group.SelectGroup();
        }*/
    }

    private void DeselectAll()
    {
        OnDeselect.Invoke(spawner);
        throw new NotImplementedException();
        /*foreach (var group in _groups)
        {
            group.DeselectGroup();
        }*/
    }

    private void OnDestroy()
    {
        selectable.OnSelect.RemoveListener(SelectAll);
        selectable.OnDeselect.RemoveListener(DeselectAll);
    }
}
