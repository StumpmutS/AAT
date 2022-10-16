using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private SelectionPriority selectionPriority;
    
    public static SelectionManager Instance;

    public Dictionary<ESelectionType, HashSet<SelectableController>> Selected { get; private set; } = new();
    public ESelectionType SelectedType => selectionPriority.GetHighestPriority(Selected.Keys);
    
    private HashSet<Hoverable> _hovered = new();
    private Vector3 _boxStartPoint;

    public event Action<IEnumerable<SelectableController>> OnSelected = delegate { };

    private void Awake()
    {
        Instance = this;
        BaseInputManager.OnLeftClickDown += SetupBox;
        BaseInputManager.OnLeftCLickUp += UpdateSelected;
    }

    private void SetupBox()
    {
        _boxStartPoint = new Vector3();
        _boxStartPoint.SetToCursorToWorldPosition();
    }

    private void UpdateSelected()
    {
        foreach (var selectable in Selected.Values.SelectMany(h => h))
        {
            selectable.CallDeselectOverrideUICheck();
        }
        
        Selected.Clear();
        
        var selectables = GetHoveredColliders().Select(c => c.GetComponent<SelectableController>());
        foreach (var selectable in selectables)
        {
            selectable.CallSelectOverrideUICheck();
            AddSelected(selectable);
        }

        OnSelected.Invoke(selectables);
    }

    private IEnumerable<Collider> GetHoveredColliders()
    {
        var results = new Collider[256];
        var layerMask = TeamManager.Instance.GetEnemyLayer(Player.TeamNumber);
        var endPoint = new Vector3();
        endPoint.SetToCursorToWorldPosition(LayerManager.Instance.GroundLayer);
        
        var halfExtents = new Vector3(Mathf.Abs(_boxStartPoint.x - endPoint.x) / 2, Mathf.Abs(_boxStartPoint.y - endPoint.y) / 2, 100);
        Physics.OverlapBoxNonAlloc(MainCameraRef.Cam.transform.position, halfExtents, 
            results, Quaternion.Euler(MainCameraRef.Cam.transform.forward), layerMask);

        return results.Where(c => c != null);
    }

    private void AddSelected(SelectableController selectable)
    {
        if (!Selected.ContainsKey(selectable.SelectionType))
        {
            Selected[selectable.SelectionType] = new HashSet<SelectableController>();
        }
        
        Selected[selectable.SelectionType].Add(selectable);
        UIManager.Instance.ActivatePanel(selectable);
        selectable.CallSelectOverrideUICheck();
    }

    private void Update()
    {
        UpdateHovered();
    }

    private void UpdateHovered()
    {
        var newHovered = GetCurrentHovered();
        
        foreach (var hovered in _hovered.Where(h => !newHovered.Contains(h)))
        {
            hovered.StopHover();
        }

        var hoverables = newHovered.ToHashSet();

        foreach (var hovered in hoverables)
        {
            hovered.Hover();
        }
        
        _hovered = hoverables;
    }

    private IEnumerable<Hoverable> GetCurrentHovered()
    {
        return GetHoveredColliders().Select(c => c.GetComponent<Hoverable>());
    }
}