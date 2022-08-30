 using System;
using System.Collections;
 using System.Collections.Generic;
 using System.Linq;
 using Fusion;
using UnityEngine;
using Utility.Scripts;

 public class FlyingController : SimulationBehaviour, IMovement
 {
     [SerializeField] private float turnEfficiency = 5;
     [SerializeField, Range(10, 200)] private int pathingQuality = 60;
     [SerializeField] private float haltDistance;
     [SerializeField] private float haltSpeed;
     [SerializeField] private float haltTime;
     [SerializeField] private float haltLerpPercent;

     private UnitStatsModifierManager _stats;
     private float _speed => _stats.GetStat(EUnitFloatStats.MovementSpeed);
     private float _hoverSpeedPerc => _stats.GetStat(EUnitFloatStats.HoverSpeedPercentMultiplier);
     private float _turnSpeed => _stats.GetStat(EUnitFloatStats.TurnSpeed);
     private float _upwardPitchCap => _stats.GetStat(EUnitFloatStats.UpwardPitchCap);

     private StumpBezier _flightPath;
     private float _distanceTraveled;
     private float _totalDistance;

     public event Action<float> OnDirectionChanged = delegate { };
     public event Action OnArrival = delegate { };

     private void OnDrawGizmosSelected()
     {
         if (_flightPath == null || _flightPath.ControlPoints.Count < 5) return;
         Gizmos.color = Color.red;
         Gizmos.DrawWireSphere(_flightPath.ControlPoints[^1], 1);
         
         Gizmos.color = Color.blue;
         for (int i = 0; i < _flightPath.BezierPoints.Count - 1; i++)
         {
             Gizmos.DrawLine(_flightPath.BezierPoints[i], _flightPath.BezierPoints[i + 1]);
         }
         
         Debug.Log((_flightPath.ControlPoints[^1] - _flightPath.ControlPoints[0]).magnitude);
     }

     private void Awake()
     {
         _stats = GetComponent<UnitStatsModifierManager>();
     }

     public void SetDestination(Vector3 destination)
     {
         if (_flightPath != null && _flightPath.ControlPoints.Count > 4 && Vector3.Distance(destination, _flightPath.ControlPoints[4]) < .01f) return;

         _flightPath = new StumpBezier(SetControlPoints(destination), pathingQuality);
         _distanceTraveled = 0;
         _totalDistance = _flightPath.ArcLength;
     }

     private List<Vector3> SetControlPoints(Vector3 destination)
     {
         var dir = destination - transform.position;
         //TODO: if dir.mag > turnEfficiency * 2.2 HOVER INSTEAD
         dir = dir.normalized;
         
         var p2 = transform.position + transform.forward * turnEfficiency;
         
         Vector3 p3;
         if (Vector3.Dot(dir, transform.forward) < 0)
         {
             if (Vector3.Dot(dir, transform.right) < 0)
             {
                 p3 = transform.position + -transform.right * 2 * turnEfficiency;
             }
             else
             {
                 p3 = transform.position + transform.right * 2 * turnEfficiency;
             }
         }
         else
         {
             p3 = transform.position + dir * 2 * turnEfficiency;
         }
         
         var p4 = transform.position + dir * (turnEfficiency * 2);
         p4 = new Vector3(p4.x, destination.y, p4.z);
         
         return new List<Vector3>
         {
             transform.position,
             p2, p3, p4,
             destination
         };
     }

     public void Move()
     {
         if (_distanceTraveled >= _totalDistance) return;
         _distanceTraveled += _speed * Runner.DeltaTime;
         if (!_flightPath.TryGetDataAtDistance(_distanceTraveled, out var position, out var tangent)) return;
         
         transform.position = position;
         var dirChange = Vector3.Angle(transform.forward, tangent);
         transform.forward = tangent;

         if (_distanceTraveled >= _totalDistance)
         {
             OnArrival.Invoke();
         }

         OnDirectionChanged.Invoke(dirChange);
     }
 }