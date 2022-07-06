using System;
using System.Collections;
using UnityEngine;
using Utility.Scripts;

public class FlyingController : MonoBehaviour
{
    [SerializeField] private float haltDistance;
    [SerializeField] private float haltSpeed;
    [SerializeField] private float haltTime;
    [SerializeField] private float haltLerpPercent;
    [SerializeField] private GameObject visuals;
    [SerializeField] private float maxVisualRotation;

    private UnitStatsModifierManager _stats;
    private float _speed => _stats.GetStat(EUnitFloatStats.MovementSpeed);
    private float _hoverSpeed => _stats.GetStat(EUnitFloatStats.HoverSpeed);
    private float _turnSpeed => _stats.GetStat(EUnitFloatStats.TurnSpeed);
    private float _upwardPitchCap => _stats.GetStat(EUnitFloatStats.UpwardPitchCap);

    private bool _flipping;

    private bool _haltDone;
    private float _currentHaltSpeed;
    private float _elapsedHaltTime;

    public event Action OnArrival = delegate { };

    private Vector3 _destination;
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_destination, 1);
    }

    private void Awake()
    {
        _stats = GetComponent<UnitStatsModifierManager>();
    }

    public void Fly(Vector3 destination)
    {
        _destination = destination;
        var destinationDirection = destination - transform.position;
        
        if (destinationDirection.magnitude < haltDistance)
        {
            Hover(destination);
            return;
        }
        
        Reset();
        
        Flip(destination);
        if (_flipping) return;
       
        transform.RotateTowardsOnY(destination, _turnSpeed, out var yAnglesTurned);
        transform.RotateTowardsOnX(destination, _turnSpeed, out var xAnglesTurned);
        CheckXAngleLimits();
        
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private bool CheckXAngleLimits()
    {
        var currentXAngle = (360 + transform.localRotation.eulerAngles.x) % 360;
        var limitAngle = (360 + -_upwardPitchCap) % 360;
        switch (currentXAngle)
        {
            case > 180:
                transform.rotation = Quaternion.Euler(Mathf.Max(limitAngle, currentXAngle),
                    transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
                return true;
            case >= 90:
                transform.rotation = Quaternion.Euler(Mathf.Min(85, currentXAngle), //TODO not hardcode
                    transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
                return true;
            default:
                return false;
        }
    }

    private void Flip(Vector3 destination)
    {
        var destinationDirection = destination - transform.position;
        if (!_flipping)
        {
            var radius = 360 / _turnSpeed * _speed / (2 * Mathf.PI);
            if (Vector3.Dot(transform.right, destinationDirection) < 0) radius = -radius;
            if ((new Vector3(destination.x, 0, destination.z) -
                 (new Vector3(transform.position.x, 0, transform.position.z) + transform.right * radius)).sqrMagnitude > radius * radius)
                return;
        }

        _flipping = true;
        var yDone = transform.RotateTowardsOnY(destination, _turnSpeed, out _); //TODO: parameters
        var xDone = transform.RotateTowardsOnX(destination, _turnSpeed, out _); //TODO: parameters
        if ((CheckXAngleLimits() || xDone) && yDone) _flipping = false;
    }

    private void Hover(Vector3 destination)
    {
        if (!_haltDone)
        {
            Halt();
            return;
        }
        var destinationDirection = destination - transform.position;
        var newPos = transform.position + destinationDirection.normalized * (_hoverSpeed * Time.deltaTime);
        var forward = transform.forward;
        forward.y = 0;
        var finishedRotation = transform.RotateTowardsOnX(destination, _turnSpeed, out _);
        CheckXAngleLimits();
        
        if ((newPos - transform.position).magnitude >= destinationDirection.magnitude)
        {
            transform.position = destination;
            if (finishedRotation) OnArrival.Invoke();
            return;
        }

        transform.position = newPos;
    }

    private void Halt()
    {
        if (_currentHaltSpeed <= 0) _currentHaltSpeed = _speed;
        _currentHaltSpeed = Mathf.Lerp(_currentHaltSpeed, haltSpeed, haltLerpPercent / 100 * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * _currentHaltSpeed, _currentHaltSpeed * Time.deltaTime);
        
        _elapsedHaltTime += Time.deltaTime;
        if (_elapsedHaltTime < haltTime) return;
        _currentHaltSpeed = 0;
        _haltDone = true;
    }

    private void Reset()
    {
        _elapsedHaltTime = 0;
        _haltDone = false;
    }
}
