using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultPositionManager : MonoBehaviour
{
    [SerializeField] private CameraTargetMovementController cameraTarget;
    [SerializeField] private List<Transform> defaultCameraPositions;

    private void Start()
    {
        InputManager.OnNumberKey0 += MoveCameraTo;
        InputManager.OnNumberKey1 += MoveCameraTo;
        InputManager.OnNumberKey2 += MoveCameraTo;
        InputManager.OnNumberKey3 += MoveCameraTo;
        InputManager.OnNumberKey4 += MoveCameraTo;
        InputManager.OnNumberKey5 += MoveCameraTo;
        InputManager.OnNumberKey6 += MoveCameraTo;
    }

    private void MoveCameraTo(int cameraPositionIndex)
    {
        if (cameraTarget.transform.position == defaultCameraPositions[0].position && cameraTarget.transform.rotation == defaultCameraPositions[0].rotation) return;
        cameraTarget.transform.position = defaultCameraPositions[cameraPositionIndex].position;
        cameraTarget.transform.rotation = defaultCameraPositions[cameraPositionIndex].rotation;
    }
}
