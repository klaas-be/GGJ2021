using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeadTargetSetter : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook freeLookCam;
    [SerializeField] Throwing.Throwable HeadThrowable;
    GameObject refGO;

    private void Start()
    {
        refGO = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (HeadThrowable.IsAttached)
        {
            freeLookCam.LookAt = HeadThrowable.transform;
            freeLookCam.Follow = HeadThrowable.transform;
        }
        else
        {
            refGO.transform.position = HeadThrowable.transform.position + Vector3.up;

            freeLookCam.LookAt = refGO.transform;
            freeLookCam.Follow = refGO.transform;
        }
    }
}
