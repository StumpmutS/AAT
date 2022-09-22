using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private List<MenuController> menus;

    private void Start()
    {
        ActivateMenu(0);
    }

    public void ActivateMenu(int index)
    {
        foreach (var menu in menus)
        {
            menu.Deactivate();
        }
        menus[0].Activate();
    }

    public void ActivateMenu(MenuController toMenu)
    {
        if (!menus.Contains(toMenu)) menus.Add(toMenu);
        
        foreach (var menu in menus)
        {
            menu.Deactivate();
        }
        toMenu.Activate();
    }

    [ContextMenu("Gather Menus")]
    private void GatherMenus()
    {
        menus = FindObjectsOfType<MenuController>().ToList();
    }
}
