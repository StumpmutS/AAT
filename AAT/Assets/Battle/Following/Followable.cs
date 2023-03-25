using System.Collections.Generic;
using UnityEngine;

public class Followable : MonoBehaviour
{
    [SerializeField] private List<EFollowType> followTypes;
    public List<EFollowType> FollowTypes => followTypes;
}