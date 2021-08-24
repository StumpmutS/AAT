using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisualsController : MonoBehaviour
{
    [SerializeField] private List<Transform> wallEnds;
    public List<Transform> WallEnds => wallEnds;
}
