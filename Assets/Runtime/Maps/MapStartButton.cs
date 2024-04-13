using System;
using Cysharp.Threading.Tasks;
using SoldByWizards.Player.Interactions;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class MapStartButton : MonoBehaviour
    {
        [SerializeField] private InteractionsManager _interactionsManager;
        [SerializeField] private TimedMapLoader _timedMapLoader;

        private void Start() => _interactionsManager.OnObjectInteract += OnObjectInteract;

        private void OnObjectInteract(Ray _, RaycastHit raycastHit)
        {
            if (!raycastHit.transform.GetComponent<MapStartButton>()) return;

            _timedMapLoader.LoadMapOnTimer().Forget();
        }

        private void OnDestroy() => _interactionsManager.OnObjectInteract -= OnObjectInteract;
    }
}
