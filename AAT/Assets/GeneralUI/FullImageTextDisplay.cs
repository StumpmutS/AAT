using TMPro;
using UnityEngine;

public class FullImageTextDisplay : FullImageDisplay
{
    [SerializeField] private TMP_Text text;
    
    public void SetText(string value)
    {
        text.text = value;
    }
}