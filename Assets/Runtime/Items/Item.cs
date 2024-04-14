using System;
using SoldByWizards.Maps;
using UnityEngine;

namespace SoldByWizards.Items
{
    public class Item : MonoBehaviour
    {
        // always set immediately upon spawn
        [NonSerialized] public ItemSO ItemSO = null!;
        [NonSerialized] public bool MarkedForSale = false;
        [NonSerialized] public float SellPrice;
        [NonSerialized] public MapObjectSpawnPoint SpawnPoint;

        // TODO: Create a portal below, do a whole fancy animation
        public void SellAnimation()
        {
            this.gameObject.SetActive(false);
        }
    }
}
