using UnityEngine;

public class LabeledLayoutDisplay : MonoBehaviour
{
    [SerializeField] private LayoutDisplay layout;
    [SerializeField] private StylizedTextDisplay label;
    
    public void SetText(string text)
    {
        label.SetText(text);
    }

    public void Add(Transform rectTransform)
    {
        layout.Add(rectTransform);
    }
}