using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageDisplay : MonoBehaviour
{
    [SerializeField] private PoolingObject poolObj;
    public PoolingObject PoolObj => poolObj;
    [SerializeField] private List<Image> availableFills;
    [SerializeField] private Image lighting;
    [SerializeField] private Image shading;
    [SerializeField] private Image border;

    public void SetStylizedImage(StylizedImage stylizedImage)
    {
        foreach (var image in availableFills)
        {
            image.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < stylizedImage.Fills.Count; i++)
        {
            availableFills[i].gameObject.SetActive(true);
            availableFills[i].sprite = stylizedImage.Fills[i]; 
            availableFills[i].color = stylizedImage.FillColors[i];
        }
    
        lighting.sprite = stylizedImage.Lighting;
        lighting.color = stylizedImage.LightingColor;
        shading.sprite = stylizedImage.Shading;
        shading.color = stylizedImage.ShadingColor;
        border.sprite = stylizedImage.Border;
        border.color = stylizedImage.BorderColor;
        border.rectTransform.offsetMin = stylizedImage.BorderOffsets.LeftBottom;
        border.rectTransform.offsetMax = stylizedImage.BorderOffsets.RightTop;
    }
}