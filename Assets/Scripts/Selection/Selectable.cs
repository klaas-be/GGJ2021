using System;
using UnityEngine;

namespace Selection
{
    public class Selectable : MonoBehaviour
    {
        [SerializeField] private Material[] selectedMaterials;
        [SerializeField] private Renderer rendererOnChange;
        

        private Material[] defaultMaterials;

        private void Start()
        {
            defaultMaterials = rendererOnChange?.sharedMaterials;
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
            rendererOnChange.sharedMaterials = materials;
        }
    }
}