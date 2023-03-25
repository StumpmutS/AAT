using System.Collections.Generic;
using UnityEngine;

public class InputAwaiter : MonoBehaviour
{
    public bool AwaitingInput => _inputBlockers.Count > 0;

    private HashSet<object> _inputBlockers = new();

    public void StartAwaitInput(object caller)
    {
        _inputBlockers.Add(caller);
    }

    public void StopAwaitInput(object caller)
    {
        _inputBlockers.Remove(caller);
    }
}