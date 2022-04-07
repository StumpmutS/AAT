using UnityEngine;
using UnityEngine.EventSystems;

public class StumpEventSystemManagerReference : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    public static StumpEventSystemManagerReference Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool OverUI() => eventSystem.IsPointerOverGameObject();
}
