using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Lever : Interactable
    {
        [Header("Refs")]
        [SerializeField] Transform leverPivot;

        [Header("Settings")]
        [SerializeField] float timeToSwitch = 1f;

        public bool DebugToggle = false;

        private void Update()
        {
            if (DebugToggle)
            {
                Interact();
                DebugToggle = false;
            }
        }

        public override void Interact()
        {
            base.Interact();
            StartCoroutine(SwitchState());
        }

        IEnumerator SwitchState()
        {
            float elapsed_time = 0;
            while (elapsed_time < timeToSwitch)
            {
                elapsed_time += Time.deltaTime;
                if (state)
                {
                    leverPivot.rotation = Quaternion.Lerp(Quaternion.Euler(0,0,225), 
                                            Quaternion.Euler(0, 0, 315), 
                                            elapsed_time / timeToSwitch);
                }
                else
                {
                    leverPivot.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 315),
                                            Quaternion.Euler(0, 0, 225),
                                            elapsed_time / timeToSwitch);
                }
                yield return null;
            }

            state = !state;
        }
    }
}