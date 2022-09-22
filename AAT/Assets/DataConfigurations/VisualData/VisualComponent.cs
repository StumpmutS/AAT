using UnityEngine;

public abstract class VisualComponent : ScriptableObject
{
    public abstract void ActivateComponent(VisualInfo info);
}