using UnityEngine;
using UnityEngine.Events;

public class Property : MonoBehaviour
{
    public UnityEvent<int> OnPropertyChanged;

    public void InvokeChange(int value)
    {
        OnPropertyChanged.Invoke(value);
    }
}