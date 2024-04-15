using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SoldByWizards.Maps
{
#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
// lightmap hack
    public class ShippingContainerMaterialSetter : MonoBehaviour
    {
        [SerializeField]
        private List<Material> _possibleMaterials = new();

        public void OnEnable()
        {
            if (!Application.isEditor)
                return;
            // randomize shipping containers
            var shippingContainers = FindObjectsOfType<ShippingContainer>();

            foreach (var shippingContainer in shippingContainers)
            {
                // random mat
                var mat = _possibleMaterials[Random.Range(0, _possibleMaterials.Count)];
                foreach (var coloredRenderer in shippingContainer.ColoredRenderers)
                {
                    coloredRenderer.sharedMaterial = mat;
                }
            }
        }
    }
}
