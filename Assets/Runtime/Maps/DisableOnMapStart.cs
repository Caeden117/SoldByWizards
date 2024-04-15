using UnityEngine;
using UnityEngine.Serialization;

namespace SoldByWizards.Maps
{
    public class DisableOnMapStart : MonoBehaviour
    {
        [SerializeField] private GameObject[] _target;
        [SerializeField] private PortalController _portalController;

        private void Start()
        {
            _portalController.OnPortalOpen += ToggleObjects;
            _portalController.OnPortalClose += ToggleObjects;
        }

        private void ToggleObjects()
        {
            foreach (var obj in _target) obj.SetActive(!obj.activeSelf);
        }
        private void OnDestroy()
        {
            _portalController.OnPortalOpen -= ToggleObjects;
            _portalController.OnPortalClose -= ToggleObjects;
        }
    }
}
