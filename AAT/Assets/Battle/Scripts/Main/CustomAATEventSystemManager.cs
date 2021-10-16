using UnityEngine;
using UnityEngine.EventSystems;

public class CustomAATEventSystemManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;

    private static CustomAATEventSystemManager instance;
    public static CustomAATEventSystemManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    public bool OverUI()
    {
        return eventSystem.IsPointerOverGameObject();
    }
}
