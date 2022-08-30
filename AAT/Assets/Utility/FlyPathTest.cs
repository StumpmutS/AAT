using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

public class FlyPathTest : MonoBehaviour
{
    [SerializeField] private float t;
    [SerializeField] private float speed = 1f;
    [SerializeField, Min(.5f)] private float ratio = 5;
    [SerializeField] private Transform p1, p2, p3, p4, p5;

    private float _arcLength;
    List<Tuple<float, float>> distAndT = new();
        
    private void Start()
    {
        var points = new List<Vector3>()
        {
            p1.transform.position, p2.transform.position, p3.transform.position, p4.transform.position,
            p5.transform.position
        };

        float arcLength = 0;
        var bezier = StumpBezierHelpers.CreateBezier(points, 10).ToList();
        distAndT.Add(new Tuple<float, float>(0, 0));
        for (int i = 0; i < bezier.Count - 1; i++)
        {
            arcLength += (bezier[i + 1] - bezier[i]).magnitude;
            distAndT.Add(new Tuple<float, float>(arcLength, (float) (i + 1) / (bezier.Count - 1)));
        }

        _arcLength = arcLength;
    }

    [SerializeField] private float distanceTraveled;
    
    private void Update()
    {
        SetPath();
        
        var points = new List<Vector3>
        {
            p1.transform.position, p2.transform.position, p3.transform.position, p4.transform.position,
            p5.transform.position
        };

        distanceTraveled += speed * Time.deltaTime;

        float currentBezierValue = 0;
        for (int i = 0; i < distAndT.Count - 1; i++)
        {
            if (distanceTraveled < distAndT[i].Item1 || distanceTraveled > distAndT[i + 1].Item1) continue;
            
            var segmentLength = distAndT[i + 1].Item1 - distAndT[i].Item1;
            var distance = distanceTraveled - distAndT[i].Item1;
            var percent = distance / segmentLength;
            currentBezierValue = Mathf.Lerp(distAndT[i].Item2, distAndT[i + 1].Item2, percent);
            break;
        }

        var tangent = StumpBezierHelpers.SampleTangent(points, currentBezierValue);
        var point = StumpBezierHelpers.SamplePoint(points, currentBezierValue);
        Debug.DrawLine(point, point + tangent);
    }

    private void SetPath()
    {
        var dir = p5.position - p1.position;
        var mag = dir.magnitude;
        dir = dir.normalized;
        p2.position = p1.forward * mag / ratio;
        p4.position = dir * mag / (ratio * 2);
        p4.position = new Vector3(p4.position.x, p5.position.y, p4.position.z);
        if (Vector3.Dot(dir, p1.forward) < 0)
        {
            if (Vector3.Dot(dir, p1.right) < 0)
            {
                p3.position = -p1.right * 2 * mag / ratio;
            }
            else
            {
                p3.position = p1.right * 2 * mag / ratio;
            }
        }
        else
        {
            p3.position = dir * 2 * mag / ratio;
        }
    }
}
