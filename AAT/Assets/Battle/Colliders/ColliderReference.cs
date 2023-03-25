using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColliderReference : MonoBehaviour, IColliderUpdater
{
    [SerializeField] private List<Collider> startColliders;
    
    private HashSet<Collider> _colliders;
    public HashSet<Collider> Colliders => _colliders;
    protected HashSet<IColliderUpdater> _colliderUpdaters = new();
    
    public event Action OnCollidersChanged;

    protected virtual void Awake()
    {
        _colliders = startColliders.ToHashSet();
    }

    private void Start()
    {
        RefreshColliders();
    }

    protected void RefreshColliders()
    {
        ClearColliderUpdaters();
        var newColliderUpdaters = GetColliderUpdaters();
        foreach (var colliderUpdater in newColliderUpdaters)
        {
            RegisterColliderUpdater(colliderUpdater);
        }

        _colliders = newColliderUpdaters.SelectMany(u => u.GetColliders()).ToHashSet();
    }

    private void ClearColliderUpdaters()
    {
        foreach (var colliderUpdater in _colliderUpdaters)
        {
            colliderUpdater.OnCollidersChanged -= RefreshColliders;
        }
        _colliderUpdaters.Clear();
    }

    protected virtual IColliderUpdater[] GetColliderUpdaters()
    {
        return GetComponentsInChildren<IColliderUpdater>();
    }

    protected void RegisterColliderUpdater(IColliderUpdater colliderUpdater)
    {
        if (_colliderUpdaters.Add(colliderUpdater))
        {
            colliderUpdater.OnCollidersChanged += RefreshColliders;
        }
    }

    public List<Collider> GetColliders()
    {
        return startColliders;
    }
}