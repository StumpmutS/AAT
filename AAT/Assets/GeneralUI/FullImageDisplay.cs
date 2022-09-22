using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FullImageDisplay : MonoBehaviour
{
    [SerializeField] private RectTransform imageContainer;
    [SerializeField] private ImageTextDisplay imageDisplayPrefab;

    private List<ImageDisplay> _currentDisplays = new();

    public void SetStylizedImages(List<StylizedTextImage> stylizedTextImages)
    {
        foreach (var display in _currentDisplays)
        {
            display.PoolObj.Deactivate();
        }
        _currentDisplays.Clear();

        foreach (var stylizedTextImage in stylizedTextImages)
        {
            var created = PoolingManager.Instance.CreatePoolingObject(imageDisplayPrefab.PoolObj).GetComponent<ImageTextDisplay>();
            created.transform.SetParent(imageContainer, false);
            created.SetStylizedImage(stylizedTextImage.Image);
            created.SetText(stylizedTextImage.Text);
        }
    }
}