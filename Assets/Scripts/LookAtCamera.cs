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
    Vector3 direction;

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
                direction = Camera.main.transform.position - transform.position;
                direction.x = 0;
                transform.rotation = Quaternion.LookRotation(direction);
                break;

            case Mode.CameraForwardInverted:
                direction = -Camera.main.transform.position - transform.position;
                direction.x = 0;
                transform.rotation = Quaternion.LookRotation(direction);
                break;
        }

    }
}
