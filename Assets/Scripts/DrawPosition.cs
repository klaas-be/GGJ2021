using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPosition : MonoBehaviour
{
    [SerializeField] Color drawColor = Color.blue;
    [SerializeField] float sphereSize = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = drawColor;
        Gizmos.DrawWireSphere(transform.position, sphereSize);
    }
}
