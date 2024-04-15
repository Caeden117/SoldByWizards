#if UNITY_EDITOR

using System;
using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Editor
{
    [Serializable]
    public class ItemCameraData
    {
        public Item Item;
        public float Height;
        public float Length;
        public float CameraRotation;
    }
}
#endif
