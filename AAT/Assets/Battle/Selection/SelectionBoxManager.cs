using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class SelectionBoxManager : Singleton<SelectionBoxManager>
{
    [SerializeField] private SelectionBox selectionBox;
    [SerializeField] private float maxSelectionDistance = 100;
    [SerializeField] private float minSelectionDistance = .1f;
    [SerializeField] private MeshCollider meshCollider;

    private HashSet<SelectionTarget> _selected = new();
    private HashSet<SelectionTarget> _lastHovered = new();
    private HashSet<SelectionTarget> _currentHovered = new();
    private Vector3 _realBoxStartPoint;
    private Vector3 _realBoxEndPoint;
    private Vector3 _fakeBoxStartPoint;
    private Vector3 _fakeBoxEndPoint;
    private bool _boxSet;
    
    protected override void Awake()
    {
        base.Awake();
        BaseInputManager.OnLeftClickDown += SetupBox;
        BaseInputManager.OnLeftCLickUp += FinishBox;
    }

    private void SetupBox()
    {
        if (UIHoveredReference.Instance.OverUI()) return;

        _realBoxStartPoint = Input.mousePosition;
        BoxSetActive(true);
        selectionBox.Activate();
    }

    private void BoxSetActive(bool value)
    {
        _boxSet = value;
        meshCollider.enabled = value;
    }

    private void FinishBox()
    {
        UpdateSelected();
        ClearHovered();
        BoxSetActive(false);
        selectionBox.Deactivate();
    }

    private void UpdateSelected()
    {
        if (_selected.Any(t => t.Selectable.InputAwaiter.AwaitingInput)) return;

        foreach (var selectionTarget in _selected)
        {
            selectionTarget.Selectable.CallDeselectOverrideUICheck();
        }

        _selected.Clear();

        _selected = new HashSet<SelectionTarget>(_lastHovered);
        foreach (var selectionTarget in _selected)
        {
            selectionTarget.Selectable.CallSelectOverrideUICheck();
        }
    }

    private void Update()
    {
        if (!_boxSet) return;
        UpdateSelectionBoxDisplay();
        UpdateSelectionCollider();
    }

    private void FixedUpdate()
    {
        if (!_boxSet) return;
        UpdateHovered();
    }

    private void UpdateHovered()
    {
        foreach (var hovered in _lastHovered.Where(t => !_currentHovered.Contains(t)))
        {
            RemoveHovered(hovered);
        }

        _lastHovered = new HashSet<SelectionTarget>(_currentHovered);
        _currentHovered.Clear();
    }

    private void UpdateSelectionBoxDisplay()
    {
        _realBoxEndPoint = Input.mousePosition;

        _fakeBoxStartPoint.x = Mathf.Min(_realBoxStartPoint.x, _realBoxEndPoint.x);
        _fakeBoxStartPoint.y = Mathf.Min(_realBoxStartPoint.y, _realBoxEndPoint.y);
        _fakeBoxEndPoint.x = Mathf.Max(_realBoxStartPoint.x, _realBoxEndPoint.x);
        _fakeBoxEndPoint.y = Mathf.Max(_realBoxStartPoint.y, _realBoxEndPoint.y);

        selectionBox.UpdateCorners(_fakeBoxStartPoint, _fakeBoxEndPoint);
    }
    
    private void UpdateSelectionCollider()
    {
        if (!_boxSet || !CheckSafeDistance()) return;
        
        var vertices = new List<Vector3>();
        var startPositions = GetWorldRayPositions(minSelectionDistance);
        var endPositions = GetWorldRayPositions(maxSelectionDistance);
        vertices.AddRange(endPositions);
        vertices.AddRange(startPositions);

        meshCollider.sharedMesh = GenerateMesh(vertices.ToArray());
    }

    private bool CheckSafeDistance()
    {
        return Mathf.Abs(_fakeBoxStartPoint.x - _fakeBoxEndPoint.x) > .01f &&
               Mathf.Abs(_fakeBoxStartPoint.y - _fakeBoxEndPoint.y) > .01f;
    }

    private Vector3[] GetWorldRayPositions(float distance)
    {
        var results = new Vector3[4];

        results[0] = MainCameraRef.Cam.ScreenPointToRay(_fakeBoxStartPoint).GetPoint(distance);
        results[1] = MainCameraRef.Cam.ScreenPointToRay(new Vector3(_fakeBoxEndPoint.x, _fakeBoxStartPoint.y)).GetPoint(distance);
        results[2] = MainCameraRef.Cam.ScreenPointToRay(new Vector3(_fakeBoxStartPoint.x, _fakeBoxEndPoint.y)).GetPoint(distance);
        results[3] = MainCameraRef.Cam.ScreenPointToRay(_fakeBoxEndPoint).GetPoint(distance);
        return results;
    }

    private Mesh GenerateMesh(Vector3[] vertices)
    {
        var selectionMesh = new Mesh
        {
            vertices = vertices,
            triangles = StumpMeshGen.HexahedronTris
        };

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_boxSet) return;
        if (other.TryGetComponent<SelectionTarget>(out var selectionTarget))
        {
            AddHovered(selectionTarget);
        }
    }

    private void AddHovered(SelectionTarget selectionTarget)
    {
        selectionTarget.Hoverable.Hover();

        _currentHovered.Add(selectionTarget);
    }

    private void RemoveHovered(SelectionTarget selectionTarget)
    {
        selectionTarget.Hoverable.StopHover();
    }

    private void ClearHovered()
    {
        foreach (var target in _lastHovered)
        {
            target.Hoverable.StopHover();
        }
        
        foreach (var target in _currentHovered)
        {
            target.Hoverable.StopHover();
        }
        
        _currentHovered.Clear();
        _lastHovered.Clear();
    }
}