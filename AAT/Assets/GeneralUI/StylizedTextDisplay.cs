using TMPro;
using UnityEngine;

public class StylizedTextDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    public void SetStylizedText(StylizedText stylizedText)
    {
        text.text = stylizedText.Text;
        text.color = stylizedText.TextColor;
        if (stylizedText.FontSize > .01f) text.enableAutoSizing = false; 
        text.fontSize = stylizedText.FontSize;
        text.rectTransform.offsetMin = stylizedText.TextOffsets.LeftBottom;
        text.rectTransform.offsetMax = stylizedText.TextOffsets.RightTop;
    }

    public void SetText(string text)
    {
        
    }
}