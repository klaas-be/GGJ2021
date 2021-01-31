using System;
using UnityEngine;
using UnityEngine.Events;

namespace Throwing
{
    public class Detacher : MonoBehaviour
    {
        [SerializeField] private Throwable throwable;
        [SerializeField] private KeyCode detachKey;

        [Header("Events")]
        public UnityEvent StartDetachEvent;
        public UnityEvent StartAttachEvent;
        public UnityEvent EndAttachEvent;

        private void Update()
        {
            if (Input.GetKeyDown(detachKey))
            {
                throwable.Detach(EndAttachEvent);
                StartDetachEvent.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("reattach");
                StartCoroutine(throwable.Reattach());
                StartAttachEvent.Invoke();
            }
        }
    }
}