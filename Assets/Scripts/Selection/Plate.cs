﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Throwing
{
    [RequireComponent(typeof(Collider))]
    public class Plate : Interactable
    {
        [Header("Refs")]
        [SerializeField] Transform platePivot;

        private Vector3 startPos;

        [Header("Settings")]
        [SerializeField] float plateDipDown = 0.05f;
        [SerializeField] float timeToSwitch = 1f;

        public bool DebugToggle = false;

        private void Start()
        {
            startPos = platePivot.position;
        }

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
                if (!state)
                    platePivot.position = Vector3.Lerp(startPos, startPos + Vector3.down * plateDipDown, elapsed_time / timeToSwitch);
                else
                    platePivot.position = Vector3.Lerp(startPos + Vector3.down * plateDipDown, startPos, elapsed_time / timeToSwitch);

                yield return null;
            }

            if (!state)            
                platePivot.position = startPos + Vector3.down * plateDipDown;            
            else            
                platePivot.position = startPos;            

            state = !state;
        }
    }
}
