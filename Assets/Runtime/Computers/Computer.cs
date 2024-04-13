using JetBrains.Annotations;
using UnityEngine;

namespace SoldByWizards.Computers
{
    public class Computer : MonoBehaviour
    {
        public Transform CameraFocusPoint = null!;
        public GameObject CustomVisualsWhenEnabled = null!;
        public Camera ComputerCamera = null!;
        public RectTransform ItemListingContainer = null!;
        public float IntendedFov = 72f;
    }
}
