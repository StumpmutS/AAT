using UnityEngine;
using UnityEngine.UI;

public abstract class DecalComponent : MonoBehaviour
{
    [SerializeField] private float delay;
    public float Delay => delay;

    public abstract void Activate();

    public abstract void Deactivate();
}