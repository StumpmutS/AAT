using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] List<Canvas> menuCanvases;

    private void Start()
    {
        menuCanvases[0].gameObject.SetActive(true);
        for (int i = 1; i < menuCanvases.Count; i++)
        {
            menuCanvases[i].gameObject.SetActive(false);
        }
    }

    public void SwitchCanvas(Canvas switchCanvas)
    {
        foreach (var canvas in menuCanvases)
        {
            if (canvas != switchCanvas)
            {
                canvas.gameObject.SetActive(false);
            }
            else
            {
                canvas.gameObject.SetActive(true);
            }
        }
    }
}
