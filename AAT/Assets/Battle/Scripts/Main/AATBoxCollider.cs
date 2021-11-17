using UnityEngine;

public class AATBoxCollider : BoxCollider
{
    [SerializeField] private GameObject gameObjectRef;
    public GameObject GameObjectRef => gameObjectRef;
}
