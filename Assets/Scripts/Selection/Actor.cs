using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Throwing
{
    public class Actor : MonoBehaviour
    {
        [SerializeField] List<Interactable> interactablesList = new List<Interactable>();
        protected bool state = false;
        bool lastState = false;
        bool activateActor = false;

        private void Update()
        {
            bool actorStateChange = true;
            foreach (Interactable item in interactablesList)
            {
                actorStateChange &= item.GetState();
            }

            activateActor = lastState != actorStateChange;

            if (activateActor)
            {
                lastState = actorStateChange;
                ActivateActor();
            }
        }

        public virtual void ActivateActor()
        {

        }
    }
}
