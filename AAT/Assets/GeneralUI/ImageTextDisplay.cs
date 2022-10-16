using UnityEngine;

public class ImageTextDisplay : MonoBehaviour
{
    [SerializeField] private PoolingObject poolObj;
    public PoolingObject PoolObj => poolObj;
    
    [SerializeField] private StylizedImageDisplay imageDisplay;
    [SerializeField] private StylizedTextDisplay textDisplay;

    public void SetImage(StylizedImage image)
    {
        imageDisplay.SetImage(image);
    }
    
    public void SetText(StylizedText stylizedText)
    {
        textDisplay.SetStylizedText(stylizedText);
    }

    public void SetTextImage(StylizedTextImage stylizedTextImage)
    {
        SetImage(stylizedTextImage.Image);
        SetText(stylizedTextImage.Text);
    }
}