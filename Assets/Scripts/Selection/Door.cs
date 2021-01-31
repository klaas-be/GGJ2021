using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Door : Actor
    {
        [Header("Settings")]
        [SerializeField] Vector3 endPosOffset;
        [SerializeField] float moveTime = 2f;

        Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
        }

        public override void ActivateActor()
        {
            base.ActivateActor();
            StartCoroutine(SwitchState());
        }
        IEnumerator SwitchState()
        {
            float elapsed_time = 0;
            while (elapsed_time < moveTime)
            {
                elapsed_time += Time.deltaTime;
                if (!state)
                {
                    transform.position = Vector3.Lerp(startPos, startPos + endPosOffset, elapsed_time / moveTime);
                }
                else
                {
                    transform.position = Vector3.Lerp(startPos + endPosOffset, startPos, elapsed_time / moveTime);
                }
                yield return null;
            }

            state = !state;
        }
    }
}