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
        [NonSerialized] public Rigidbody Rigidbody;

        // TODO: Create a portal below, do a whole fancy animation
        public void SellAnimation() => Rigidbody.excludeLayers = ~0;
    }
}
