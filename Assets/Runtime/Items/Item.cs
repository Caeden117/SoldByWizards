using System;
using UnityEngine;

namespace SoldByWizards.Items
{
    public class Item : MonoBehaviour
    {
        // always set immediately upon spawn
        [NonSerialized] public ItemSO ItemSO = null!;
        [NonSerialized] public bool MarkedForSale = false;
        [NonSerialized] public float SellPrice;

        // :3
        private void LateUpdate()
        {
            if (transform.position.y < -100) DestroyImmediate(gameObject);
        }

        // TODO: Create a portal below, do a whole fancy animation
        public void SellAnimation()
        {
            this.gameObject.SetActive(false);
        }
    }
}
