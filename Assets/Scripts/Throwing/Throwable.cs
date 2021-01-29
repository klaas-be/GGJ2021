using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Throwable : MonoBehaviour
    {

        public Transform anchor;
        public Transform ikTarget; 

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

            float elapse_time = 0;

            while (Vector3.Distance(anchor.transform.position, transform.position) > 0.1f)
            {
                elapse_time += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, anchor.transform.position, elapse_time);
                yield return null;
            }


            ikTarget.position = originalTargetPosition;
            
            transform.parent = parent;
            transform.position = anchor.position;
        }

        /// <summary>
        /// Unparrent the game object
        /// </summary>
        public void Detach()
        {
            parent = null;
        }

        public void MoveIkTargetToTarget(Vector3 targetPosition)
        {
            originalTargetPosition = ikTarget.position;
            ikTarget.position = targetPosition;

        }
    }
}