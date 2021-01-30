using System;
using UnityEngine;

namespace Throwing
{
    public class Detecher : MonoBehaviour
    {
        [SerializeField] private Throwable throwable;
        [SerializeField] private KeyCode detachKey;
        
        
        private void Update()
        {
            if (Input.GetKeyDown(detachKey))
            {
                throwable.Detach();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("reattach");
                StartCoroutine(throwable.Reattach());
            }
        }
    }
}