using System;
using System.Collections.Generic;
using Throwing;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class Weight : MonoBehaviour
    {

        public float influenceRadius;
        public Throwable throwable;

        private Plate pushedPlate; 
        private void Update()
        {
            Collider[] touchingColliders = Physics.OverlapSphere(transform.position, influenceRadius);

            bool foundPlate = false;
            
            foreach (var coll in touchingColliders)
            {
                var interactable = coll.gameObject.GetComponent<Plate>();
                if (interactable != null)
                {
                    foundPlate = true;
                    if (pushedPlate == null)
                    {
                        pushedPlate = interactable;
                        pushedPlate.LinkThrowable(throwable);
                    }
                }

                if (foundPlate == false && pushedPlate !=null)
                {
                    pushedPlate.UnlinkThrowable(throwable);
                    pushedPlate = null; 
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, influenceRadius);
        }
    }
}