using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoveredReference : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private List<GraphicRaycaster> raycasters;
    public static UIHoveredReference Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool OverUI()
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };

        var useless = new List<RaycastResult>();
        
        foreach (var raycaster in raycasters)
        {
            raycaster.Raycast(pointerEventData, useless);
            if (useless.Count > 0) return true;
        }

        return false;
    }
}
