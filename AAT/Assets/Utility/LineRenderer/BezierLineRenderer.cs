using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.Scripts;

[ExecuteAlways]
public class BezierLineRenderer : StumpLineRenderer
{
    [SerializeField] [Range(1, 100)] private int quality;
    [SerializeField] private List<Transform> transforms;

    private StumpBezier _bezier;
    
    protected override void Update()
    {
        SetPoints(new StumpBezier(transforms.Where(t => t != null).Select(t => t.localPosition), quality).BezierPoints);
        base.Update();
    }
}
