using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraRotationSpeed;

    private void Start()
    {
        targetTransform.DetachChildren();
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, cameraMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetTransform.rotation, cameraRotationSpeed * Time.deltaTime);
    }
}
