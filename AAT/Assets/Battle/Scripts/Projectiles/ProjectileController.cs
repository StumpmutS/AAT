using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : NetworkBehaviour
{
    [SerializeField] protected Vector3 initialDirection;
    [SerializeField] private float initialVelocity;
    [SerializeField] private List<ProjectileComponentData> projectileComponents;

    public TeamController Team { get; private set; }
    
    protected Rigidbody _rigidBody;
    private float _damage;
    private Collider[] _origin;
    private bool _projectileFired;
    private Vector3 _firerDirection;
    private float _firerSpeed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidBody.velocity += initialDirection * initialVelocity;
    }

    private void FixedUpdate()
    {
        if (!_projectileFired) return;
        MoveProjectile();
    }

    protected virtual void MoveProjectile() { }

    public void FireProjectile(float damage, Collider[] fromColliders, Vector3 firerDirection, float firerSpeed = 0)
    {
        _damage = damage;
        _origin = fromColliders;
        _firerDirection = firerDirection.normalized;
        _firerSpeed = firerSpeed;
        _projectileFired = true;
        _rigidBody.velocity += _firerDirection * _firerSpeed;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (_origin.Contains(hit)) return;
        var hitGameObject = hit.gameObject;
        foreach (var component in projectileComponents)
        {
            component.ActivateComponent(this, hitGameObject, _damage);
            StartCoroutine(DeactivateComponentCoroutine(component, hitGameObject));
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DeactivateComponentCoroutine(ProjectileComponentData projectileComponentData, GameObject hit)
    {
        yield return new WaitForSeconds(projectileComponentData.ComponentTime);
        projectileComponentData.DeactivateComponent(hit);
    }
}
