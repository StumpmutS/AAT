using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultPositionManager : MonoBehaviour
{
    [SerializeField] private CameraTargetMovementController cameraTarget;
    [SerializeField] private List<Transform> defaultCameraPositions;

    private void Start()
    {
        InputManager.OnAlpha0 += MoveCameraTo;
        InputManager.OnAlpha1 += MoveCameraTo;
        InputManager.OnAlpha2 += MoveCameraTo;
        InputManager.OnAlpha3 += MoveCameraTo;
        InputManager.OnAlpha4 += MoveCameraTo;
        InputManager.OnAlpha5 += MoveCameraTo;
        InputManager.OnAlpha6 += MoveCameraTo;
    }

    private void MoveCameraTo(int cameraPositionIndex)
    {
        if (cameraTarget.transform.position == defaultCameraPositions[0].position && cameraTarget.transform.rotation == defaultCameraPositions[0].rotation) return;
        cameraTarget.transform.position = defaultCameraPositions[cameraPositionIndex].position;
        cameraTarget.transform.rotation = defaultCameraPositions[cameraPositionIndex].rotation;
    }
}
