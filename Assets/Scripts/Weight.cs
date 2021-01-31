using System;
using Throwing;
using UnityEngine;

namespace DefaultNamespace
{
    public class Weight : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var interactable = other.gameObject.GetComponent<Plate>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}