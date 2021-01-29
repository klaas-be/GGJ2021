using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Throwable : MonoBehaviour
    {

        public Transform anchor;

        private Transform parent;

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
    }
}