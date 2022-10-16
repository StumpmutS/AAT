using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StylizedImageDisplay : MonoBehaviour
{
    [SerializeField] private PoolingObject fillPrefab;
    [SerializeField] private Transform fillContainer;
    [SerializeField] private Image lighting;
    [SerializeField] private Image shading;
    [SerializeField] private Image border;

    private List<PoolingObject> _activeFills = new();

    public void SetImage(StylizedImage stylizedImage)
    {
        foreach (var image in _activeFills)
        {
            image.Deactivate();
        }
        
        for (int i = 0; i < stylizedImage.Fills.Count; i++)
        {
            var poolObj = PoolingManager.Instance.CreatePoolingObject(fillPrefab);
            poolObj.transform.SetParent(fillContainer, false);
            _activeFills.Add(poolObj);
            var image = poolObj.GetComponent<Image>();
            image.sprite = stylizedImage.Fills[i];
            image.color = stylizedImage.FillColors[i];
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