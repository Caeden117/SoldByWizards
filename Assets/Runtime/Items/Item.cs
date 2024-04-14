using System;
using UnityEngine;

namespace SoldByWizards.Items
{
    public class Item : MonoBehaviour
    {
        // always set immediately upon spawn
        [NonSerialized]
        public ItemSO ItemSO = null!;

        // :3
        private void LateUpdate()
        {
            if (transform.position.y < -100) DestroyImmediate(gameObject);
        }
    }
}
