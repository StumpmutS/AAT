using UnityEngine;
using UnityEngine.EventSystems;

public class CustomAATEventSystemManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    public static CustomAATEventSystemManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool OverUI() => eventSystem.IsPointerOverGameObject();
}
