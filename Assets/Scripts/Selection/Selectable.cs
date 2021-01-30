using System;
using UnityEngine;

namespace Selection
{
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private Material[] selectedMaterials;

        private Material[] defaultMaterials;

        private void Start()
        {
            defaultMaterials = GetComponent<Renderer>().sharedMaterials;
        }

        public void Select()
        {
            SetMaterials(selectedMaterials);
        }

        public void Deselect()
        {
            SetMaterials(defaultMaterials);
        }

        private void SetMaterials(Material[] materials)
        {
            GetComponent<Renderer>().sharedMaterials = materials;
        }
    }
}