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
                Interact(!state);
                DebugToggle = false;
            }
        }

        public override void Interact(bool toState)
        {
            base.Interact(toState);
            StartCoroutine(SwitchState(toState));
        }

        IEnumerator SwitchState(bool toState)
        {
            float elapsed_time = 0;
            while (elapsed_time < timeToSwitch)
            {
                elapsed_time += Time.deltaTime;
                if (toState)
                {
                    leverPivot.localRotation = Quaternion.Lerp(Quaternion.Euler(0,0,-45), 
                                            Quaternion.Euler(0, 0, 45), 
                                            elapsed_time / timeToSwitch);
                }
                else
                {
                    leverPivot.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 45),
                                            Quaternion.Euler(0, 0, -45),
                                            elapsed_time / timeToSwitch);
                }
                yield return null;
            }

            state = toState;
        }
    }
}