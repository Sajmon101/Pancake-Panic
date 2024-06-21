using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private enum Mode
    {
        LookAtCamera,
        LookAtCameraInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    void LateUpdate()
    {
        switch (mode)
        {
            case Mode.LookAtCamera:
                transform.LookAt(Camera.main.transform);
                break;

            case Mode.LookAtCameraInverted:
                Vector3 CameraInvertedDir = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + CameraInvertedDir);
                break;

            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;

            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }

    }
}
