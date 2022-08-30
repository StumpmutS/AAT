using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility.Scripts
{
    public class StumpBezier
    {
        public List<Vector3> ControlPoints { get; private set; }
        public List<Vector3> BezierPoints { get; private set; } = new();
        public float ArcLength => _tIntervalsByDistance[^1].Item2;

        private List<Tuple<float, float>> _tIntervalsByDistance = new();

        public StumpBezier(IEnumerable<Vector3> points, int quality)
        {
            ControlPoints = new(points);
            
            for (int i = 0; i <= quality; i++)
            {
                BezierPoints.Add(StumpBezierHelpers.SamplePoint(ControlPoints, (float) i / quality));
            }

            float currentDistance = 0;
            _tIntervalsByDistance.Add(new Tuple<float, float>(0, 0));
            for (int i = 0; i < BezierPoints.Count - 1; i++)
            {
                currentDistance += (BezierPoints[i + 1] - BezierPoints[i]).magnitude;
                _tIntervalsByDistance.Add(new Tuple<float, float>((float) (i + 1) / (BezierPoints.Count - 1), currentDistance));
            }
        }
        
        public float GetT(float distance)
        {
            for (int i = 0; i < _tIntervalsByDistance.Count - 1; i++)
            {
                if (distance < _tIntervalsByDistance[i].Item2 || distance > _tIntervalsByDistance[i + 1].Item2) continue;
                
                var segmentLength = _tIntervalsByDistance[i + 1].Item2 - _tIntervalsByDistance[i].Item2;
                var segmentDistance = distance - _tIntervalsByDistance[i].Item2;
                var percent = segmentDistance / segmentLength;
                return Mathf.Lerp(_tIntervalsByDistance[i].Item1, _tIntervalsByDistance[i + 1].Item1, percent);
            }

            return -1;
        }

        public Vector3 GetPositionAtDistance(float distance)
        {
            var t = GetT(distance);

            return StumpBezierHelpers.SamplePoint(ControlPoints, t);
        }

        public Vector3 GetTangentAtDistance(float distance)
        {
            var t = GetT(distance);

            return StumpBezierHelpers.SampleTangent(ControlPoints, t);
        }

        public bool TryGetDataAtDistance(float distance, out Vector3 position, out Vector3 tangent)
        {
            var t = GetT(distance);

            position = StumpBezierHelpers.SamplePoint(ControlPoints, t);
            tangent = StumpBezierHelpers.SampleTangent(ControlPoints, t);

            return t >= 0;
        }
    }
}
