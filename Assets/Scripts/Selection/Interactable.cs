using System;
using Selection;
using UnityEngine;

namespace Throwing
{
    public class Interactable : Selectable
    {
        private Throwable connectedTrowable;
        public void UnlinkThrowable(Throwable projectile)
        {
            connectedTrowable = null;
            projectile.ConnectedInteractable = null; 
            Debug.Log("linked "+projectile+"with "+this.name);
        }


        public void linkedThrowable(Throwable projectile)
        {
            projectile.ConnectedInteractable = this;
            connectedTrowable = null;
            Debug.Log("Unlinked "+projectile+"from "+this.name);
        }

        public void Interact()
        {
            throw new NotImplementedException();
        }
    }
}