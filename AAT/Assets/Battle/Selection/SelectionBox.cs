using System;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private bool _active;
    
    public void UpdateCorners(Vector3 startPoint, Vector3 endPoint)
    {
        startPoint.y = Screen.height - startPoint.y;
        _startPoint = startPoint;
        endPoint.y = Screen.height - endPoint.y;
        _endPoint = endPoint;
    }

    private void OnGUI()
    {
        if (!_active) return;
        
        GUI.DrawTexture(new Rect(_startPoint, _endPoint - _startPoint), Texture2D.grayTexture);
    }

    public void Activate()
    {
        _active = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _active = false;
        gameObject.SetActive(false);
    }
}