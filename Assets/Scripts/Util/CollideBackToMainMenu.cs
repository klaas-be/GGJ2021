using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CollideBackToMainMenu : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.GameWon();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.GameWon();
    }
}
