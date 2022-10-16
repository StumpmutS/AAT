using System.Collections.Generic;
using UnityEngine;

public class CameraDefaultPositionManager : MonoBehaviour
{
    [SerializeField] private CameraTargetMovementController cameraTarget;
    [SerializeField] private List<Transform> defaultCameraPositions;

    private void Start()
    {
        BaseInputManager.OnAlpha0 += MoveCameraTo;
        BaseInputManager.OnAlpha1 += MoveCameraTo;
        BaseInputManager.OnAlpha2 += MoveCameraTo;
        BaseInputManager.OnAlpha3 += MoveCameraTo;
        BaseInputManager.OnAlpha4 += MoveCameraTo;
        BaseInputManager.OnAlpha5 += MoveCameraTo;
        BaseInputManager.OnAlpha6 += MoveCameraTo;
    }

    private void MoveCameraTo(int cameraPositionIndex)
    {
        if (cameraTarget.transform.position == defaultCameraPositions[0].position && cameraTarget.transform.rotation == defaultCameraPositions[0].rotation) return;
        cameraTarget.transform.position = defaultCameraPositions[cameraPositionIndex].position;
        cameraTarget.transform.rotation = defaultCameraPositions[cameraPositionIndex].rotation;
    }
}
