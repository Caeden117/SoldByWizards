using System;
using UnityEngine;

namespace SoldByWizards.Items
{
    public class Item : MonoBehaviour
    {
        [NonSerialized]
        public ItemSO? ItemSO;

        // :3
        private void LateUpdate()
        {
            if (transform.position.y < -100) DestroyImmediate(gameObject);
        }
    }
}
