using SoldByWizards.Items;
using SoldByWizards.Player;
using UnityEngine;

namespace SoldByWizards
{
    public class PlayerSafetyController : MonoBehaviour
    {
        [SerializeField] private ItemsManager _itemsManager;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _safetyTeleportPoint;

        private void LateUpdate()
        {
            if (_playerController.transform.position.y >= -50) return;

            // idiot.
            _itemsManager.DropAllItems();
            _playerController.Stop();
            _playerController.Rigidbody.position = _safetyTeleportPoint.position;
            _playerCamera.transform.rotation = _safetyTeleportPoint.rotation;
        }
    }
}
