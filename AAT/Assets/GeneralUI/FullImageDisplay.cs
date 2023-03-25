using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FullImageDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform imageContainer;
    [FormerlySerializedAs("imageDisplayPrefab")] [SerializeField] private ImageTextDisplay imageTextDisplayPrefab;

    private List<PoolingObject> _currentDisplays = new();

    public void SetStylizedImages(List<StylizedTextImage> stylizedTextImages)
    {
        ClearDisplay();

        foreach (var stylizedTextImage in stylizedTextImages)
        {
            CreateImage(stylizedTextImage);
        }
    }

    private void CreateImage(StylizedTextImage stylizedTextImage)
    {
        var created = PoolingManager.Instance.CreatePoolingObject(imageTextDisplayPrefab.PoolObj);
        _currentDisplays.Add(created);
        var display = created.GetComponent<ImageTextDisplay>();
        display.transform.SetParent(imageContainer, false);
        display.SetImage(stylizedTextImage.Image);
        display.SetText(stylizedTextImage.Text);
    }

    private void ClearDisplay()
    {
        foreach (var display in _currentDisplays)
        {
            display.Deactivate();
        }
        _currentDisplays.Clear();
    }
}