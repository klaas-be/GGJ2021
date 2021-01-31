using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class BlueLineSettings : Actor
    {
        Material BlueLineOnMaterial;
        Material BlueLineOffMaterial;

        private void Start()
        {
            BlueLineOnMaterial = Resources.Load<Material>("Materials/DecorationLine_EmissionOn");
            BlueLineOffMaterial = Resources.Load<Material>("Materials/DecorationLine_EmissionOff");

            SetOff();
        }

        public override void ActivateActor()
        {
            base.ActivateActor();
            state = !state;
            if (state)
                SetOn();
            else
                SetOff();
        }

        public void SetOn()
        {
            this.GetComponent<Renderer>().sharedMaterial = BlueLineOnMaterial;
        }
        public void SetOff()
        {
            this.GetComponent<Renderer>().sharedMaterial = BlueLineOffMaterial;
        }
    }
}