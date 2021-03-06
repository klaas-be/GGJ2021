using System;
using UnityEngine;

namespace Selection
{
    public class SelectionManager : MonoBehaviour
    {
        public static Selectable Selected { get; private set; }
        public  LayerMask RaycastIgnoreLayer;
        private static int layerMask; 

        private void Start()
        {
            //int lm = RaycastIgnoreLayer;
            //invert layer mask
            //layerMask = 1 << lm; 
           // layerMask = ~layerMask;

           layerMask = RaycastIgnoreLayer;
        }

        private void Update()
        {
            if (RaycastFoundSelectable(out var selectable)) return;


            var hitSelectable = selectable != null;

            if (hitSelectable)
                OnSelectableHit(selectable);
            else
                OnNoSelectableHit();
        }

        
        private void OnNoSelectableHit()
        {
            if (Selected != null)
            {
                Selected.Deselect();
                Selected = null;
            }
        }

        private void OnSelectableHit(Selectable selectionComponent)
        {
            selectionComponent.Select();
            Selected = selectionComponent;
        }
        
        
        private static bool RaycastFoundSelectable(out Selectable selectionComponent)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                selectionComponent = null;
                return true;
            }

            var selection = hit.transform;
            selectionComponent = selection.GetComponent<Selectable>();
            return false;
        }
    }
}