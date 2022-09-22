using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialAlphaSpringListener : FloatSpringListener
{
    [SerializeField] private ERangeContainment rangeContainType;
    [SerializeField] private List<Renderer> renderers;
    
    protected override float GetOrig()
    {
        return renderers[0].material.color.a;
    }

    protected override void ChangeValue(float value, float target)
    {
        foreach (var render in renderers)
        {
            List<Material> updatedMats = new();
            
            foreach (var material in render.materials)
            {
                var color = material.color;
                color.a = rangeContainType == ERangeContainment.ClampZero ? ClampZero(value) : Mathf.Abs(value);
                material.color = color;
                updatedMats.Add(material);
            }

            render.materials = updatedMats.ToArray();
        }
    }

    private float ClampZero(float value)
    {
        if (value < 0) value = 0;
        return value;
    }
}
