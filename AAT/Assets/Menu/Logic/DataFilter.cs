using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataFilter : MonoBehaviour
{
    public abstract List<StumpData> FilterData(List<StumpData> stumpData);
}