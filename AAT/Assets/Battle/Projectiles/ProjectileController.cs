using  System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : NetworkBehaviour
{
    [SerializeField] protected Vector3 initialDirection;
    [SerializeField] private float initialVelocity;
    [SerializeField] private Vector3 initialAngularVelocity;
    [SerializeField] protected bool randomRotation;
    [SerializeField, ShowIf(nameof(randomRotation), true)] private float randomRange;
    [SerializeField] private List<ProjectileComponentData> projectileComponents;
    [SerializeField] private SerializableDictionary<VisualComponent, VisualInfo> visualComponents;

    public TeamController Team { get; private set; }
    
    protected Rigidbody _rigidBody;
    private float _damage;
    private HashSet<Collider> _origin;
    private bool _projectileFired;
    private Vector3 _firerDirection;
    private float _firerSpeed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        Team = GetComponent<TeamController>();
    }

    public void Init(int teamNumber)
    {
        Team.SetTeamNumber(teamNumber);
    }

    private void Start()
    {
        _rigidBody.velocity += initialDirection * initialVelocity;
    }

    public override void FixedUpdateNetwork()
    {
        if (!Runner.IsServer || !_projectileFired) return;
        MoveProjectile();
    }

    protected virtual void MoveProjectile() { }

    public void FireProjectile(float damage, IEnumerable<Collider> fromColliders, Vector3 firerDirection, float firerSpeed = 0)
    {
        _damage = damage;
        _origin = fromColliders.ToHashSet();
        _firerDirection = firerDirection.normalized;
        _firerSpeed = firerSpeed;
        _projectileFired = true;
        _rigidBody.velocity += _firerDirection * _firerSpeed;
        _rigidBody.angularVelocity += initialAngularVelocity;
        if (randomRotation)
        {
            _rigidBody.angularVelocity += new Vector3(Random.Range(0, randomRange), Random.Range(0, randomRange), Random.Range(0, randomRange));
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (_origin.Contains(hit)) return;

        foreach (var (component, info) in visualComponents)
        {
            var newInfo = info;
            newInfo.Position = transform.position;
            newInfo.Rotation = transform.rotation;
            component.ActivateComponent(newInfo);
        }
        
        if (!Runner.IsServer) return;
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
