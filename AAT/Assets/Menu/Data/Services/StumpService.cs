using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class StumpService : MonoBehaviour
{
    public abstract Task<List<StumpData>> RequestData();
}