//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Throwing
{
    public class Throwable : MonoBehaviour
    {

        public Transform anchor;
        public Transform ikTarget;

        public bool IsAttached { get { return transform.parent != null; } }
        public Interactable ConnectedInteractable { get; set; }

        private Transform parent;
        private Vector3 originalTargetPosition;
        private Rigidbody rb;
        private Collider coll;

        private UnityEvent endReattachEvent;

        private void Start()
        {
            parent = transform.parent;
            coll = this.GetComponent<Collider>();
        }

        public void StartReattach()
        {
            StartCoroutine(Reattach());
        }

        /// <summary>
        ///reattaches body part back to anchor
        /// </summary>
        public IEnumerator Reattach()
        {
            Debug.Log(""+name+"is Attached: "+IsAttached);
            if (rb != null)
            {
                rb.useGravity = false;
            }
           
            if (IsAttached) yield  break;
            

            float elapse_time = 0;

            while (Vector3.Distance(anchor.transform.position, transform.position) > 0.1f)
            {
                elapse_time += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, anchor.transform.position, elapse_time);
                yield return null;
            }


            if(ikTarget != null)
                ikTarget.position = originalTargetPosition;
            
            transform.parent = parent;
            transform.position = anchor.position;

            endReattachEvent.Invoke();
            transform.rotation = anchor.rotation;


            if (coll != null)
                coll.isTrigger = true;

            if (rb != null)
                rb.isKinematic = false;
        }

        /// <summary>
        /// Unparrent the game object
        /// </summary>
        public void Detach(UnityEvent _endReattachEvent)
        {
            transform.parent = null;
            endReattachEvent = _endReattachEvent;
        }

        public void MoveIkTargetToTarget(Vector3 targetPosition)
        {
            if (ikTarget == null) return; 
            originalTargetPosition = ikTarget.position;
            ikTarget.position = targetPosition;

        }
    }
}