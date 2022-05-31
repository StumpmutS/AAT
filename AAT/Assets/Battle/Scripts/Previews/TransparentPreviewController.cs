using UnityEngine;

public class TransparentPreviewController : MonoBehaviour
{
    [SerializeField] private TwoColorsData colors;
    [SerializeField] private Renderer previewRenderer;

    public void SetValid()
    {
        foreach (var mat in previewRenderer.materials)
        {
            mat.color = colors.Color1;
        }
    }

    public void SetInvalid()
    {
        foreach (var mat in previewRenderer.materials)
        {
            mat.color = colors.Color2;
        }
    }
}
