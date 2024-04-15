using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using SoldByWizards.Player.Interactions;
using SoldByWizards.Util;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class MapStartButton : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        [SerializeField] private InteractionsManager _interactionsManager;
        [SerializeField] private TimedMapLoader _timedMapLoader;

        private readonly RecyclableCancellationTokenSource _recyclableCancellationToken = new();

        private UniTask? _mapTask;

        private void Start()
        {
            _interactionsManager.OnButtonInteract += OnButtonInteract;
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnButtonInteract(Ray _, RaycastHit raycastHit)
        {
            if (!raycastHit.transform.GetComponent<MapStartButton>()) return;

            ActivateButton();
        }

        [PublicAPI]
        public void ActivateButton()
        {
            if (_mapTask is { Status: UniTaskStatus.Pending })
            {
                if (_timedMapLoader.TimeElapsed == 0) return;

                _recyclableCancellationToken.Cancel();
                _mapTask = null;
                return;
            }

            _mapTask = _timedMapLoader.LoadMapOnTimer(_recyclableCancellationToken.Token);
        }

        private void OnDestroy() => _interactionsManager.OnButtonInteract -= OnButtonInteract;
    }
}
