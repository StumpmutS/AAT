using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportableData : MonoBehaviour
{
    [SerializeField] private SelfOtherStatsData selfOtherStatsData;
    public SelfOtherStatsData SelfOtherStatsData => selfOtherStatsData;
}
