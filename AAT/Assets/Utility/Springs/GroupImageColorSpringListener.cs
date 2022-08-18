using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GroupImageColorSpringListener : ColorSpringListener
{
    [SerializeField] private List<Image> images;

    protected override Color GetOrig()
    {
        return images.First().color;
    }

    protected override void ChangeValue(Color value)
    {
        foreach (var image in images)
        {
            image.color = value;
        }
    }
}
