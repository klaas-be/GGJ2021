using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 rotateAxis = Vector3.up;
    public float rotateSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(rotateAxis, rotateSpeed * Time.deltaTime);
    }
}
