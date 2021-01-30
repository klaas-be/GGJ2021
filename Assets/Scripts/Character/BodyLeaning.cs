using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyLeaning : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerMovement movementScriptRef;
    [SerializeField] Transform bodyPivotTransform;

    [Header("Settings")]
    public float leanAngle = 30f;

    private Vector3 moveDir = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        moveDir = movementScriptRef.GetMotionVector().normalized;

        bodyPivotTransform.localRotation = Quaternion.Euler(moveDir.z * leanAngle, 0, moveDir.x * -leanAngle);
    }
}
