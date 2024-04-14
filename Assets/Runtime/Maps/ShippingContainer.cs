using System.Collections.Generic;
using UnityEngine;

namespace SoldByWizards.Maps
{
    // used for editor hackery to get random materials (lightmap baking no worky)
    public class ShippingContainer : MonoBehaviour
    {
        public List<Renderer> ColoredRenderers = new();
    }
}
