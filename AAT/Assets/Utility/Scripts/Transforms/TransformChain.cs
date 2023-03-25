using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformChain
{
    public List<Transform> Transforms { get; private set; }

    public TransformChain(IEnumerable<Transform> transforms)
    {
        Transforms = transforms.ToList();
    }
}