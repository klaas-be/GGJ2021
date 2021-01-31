using System;
using Selection;
using UnityEngine;
using UnityEngine.Events;

namespace Throwing
{    public class Interactable : Selectable
    {
        private Throwable connectedThrowable;
        public UnityEvent InteractableEvent;

        public Throwable ConnectedThrowable => connectedThrowable; 

        protected bool state = false;

        public void UnlinkThrowable(Throwable projectile)
        {
            Interact();
            connectedThrowable = null;
            projectile.ConnectedInteractable = null; 
            Debug.Log("Unlinked " + projectile + "from " + this.name);
           
        }


        public void LinkThrowable(Throwable projectile)
        {
            projectile.ConnectedInteractable = this;
            connectedThrowable = projectile;
            Debug.Log("Linked " + projectile + "with " + this.name);

            Interact();
        }
        public virtual void Interact()
        {
        }

        public bool GetState()
        {
            return state;
        }
    }
}