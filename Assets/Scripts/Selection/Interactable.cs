using System;
using System.Collections.Generic;
using Selection;
using UnityEngine;
using UnityEngine.Events;

namespace Throwing
{    public class Interactable : Selectable
    {
        private List<Throwable> connectedThrowables = new List<Throwable>();
        public UnityEvent InteractableEvent;


        protected bool state = false;

        public void UnlinkThrowable(Throwable projectile)
        {

            connectedThrowables.Remove(projectile);
            projectile.ConnectedInteractable = null; 
            Debug.Log("Unlinked " + projectile + "from " + this.name);

            if (connectedThrowables.Count == 0)
                Interact(false);
        }

        public void LinkThrowable(Throwable projectile)
        {
            //Aktiviere wenn eins hinzu kommt aber nur wenn es noch keins hatte
            if (connectedThrowables.Count == 0)
            {
                Interact(true);
            }

            projectile.ConnectedInteractable = this;
            connectedThrowables.Add(projectile);
            Debug.Log("Linked " + projectile + "with " + this.name);

        }

        public virtual void Interact(bool toState)
        {
        }

        public bool GetState()
        {
            return state;
        }
    }
}