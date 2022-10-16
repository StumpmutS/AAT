using UnityEngine;

public class TransparentPreviewController : MonoBehaviour
{
    [SerializeField] private ColorsData colors;
    [SerializeField] private Renderer previewRenderer;

    public void SetValid()
    {
        foreach (var mat in previewRenderer.materials)
        {
            mat.color = colors.Colors[0];
        }
    }

    public void SetInvalid()
    {
        foreach (var mat in previewRenderer.materials)
        {
            mat.color = colors.Colors[1];
        }
    }
}
