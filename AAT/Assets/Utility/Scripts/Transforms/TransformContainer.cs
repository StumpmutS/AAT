using System.Collections.Generic;
using UnityEngine;

public class TransformContainer : MonoBehaviour
{
    [Tooltip("Used to reference transforms for things like abilities, 0 should be main transform")]
    [SerializeField] private List<Transform> transforms;

    public TransformChain ToChain()
    {
        return new TransformChain(transforms);
    }
}