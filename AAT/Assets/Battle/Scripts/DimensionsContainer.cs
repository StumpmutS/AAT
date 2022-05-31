using UnityEngine;

public class DimensionsContainer : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField] private float xDimensionsStart, xDimensionsEnd, yDimensionsStart, yDimensionsEnd, zDimensionsStart, zDimensionsEnd;
    [HideInInspector] [SerializeField] private float xDimensions, yDimensions, zDimensions;

    public float XDimensions => xDimensions;
    public float YDimensions => yDimensions;
    public float ZDimensions => zDimensions;
    
    private void UpdateValues()
    {
        xDimensions = Mathf.Abs(xDimensionsStart - xDimensionsEnd);
        yDimensions = Mathf.Abs(yDimensionsStart - yDimensionsEnd);
        zDimensions = Mathf.Abs(zDimensionsStart - zDimensionsEnd);
    }

    public void OnBeforeSerialize()
    {
        UpdateValues();
    }

    public void OnAfterDeserialize()
    {
        UpdateValues();
    }
}
