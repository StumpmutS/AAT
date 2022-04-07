using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpEntity : MonoBehaviour
{
    [Tooltip("Team numbers start at 1, 0 is no team")] [SerializeField] private int teamNumber;

    public int GetTeam() => teamNumber;
}
