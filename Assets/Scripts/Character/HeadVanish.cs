using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadVanish : MonoBehaviour
{
    [SerializeField] Transform CameraTransform;
    [SerializeField] Renderer HeadRenderer;
    [SerializeField] float minDist = 0.2f;


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(CameraTransform.position, this.transform.position) < minDist)
        {
            HeadRenderer.enabled = false;
        }
        else
        {
            HeadRenderer.enabled = true;
        }
        
    }
}
