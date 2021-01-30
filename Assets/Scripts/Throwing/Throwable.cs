//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Throwable : MonoBehaviour
    {

        public Transform anchor;
        public Transform ikTarget;

        public bool IsAttached { get { return transform.parent != null; } }
        private bool isDetached; 

        private Transform parent;
        private Vector3 originalTargetPosition; 
        

        private void Start()
        {
            parent = transform.parent;
        }

        /// <summary>
        ///reattaches body part back to anchor
        /// </summary>
        public IEnumerator Reattach()
        {
            Debug.Log(""+name+"is Attached: "+IsAttached);
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
            isDetached = false;
        }

        /// <summary>
        /// Unparrent the game object
        /// </summary>
        public void Detach()
        {
            transform.parent = null;
             isDetached = true;
        }

        public void MoveIkTargetToTarget(Vector3 targetPosition)
        {
            if (ikTarget == null) return; 
            originalTargetPosition = ikTarget.position;
            ikTarget.position = targetPosition;

        }
    }
}