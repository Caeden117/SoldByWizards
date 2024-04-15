using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoldByWizards.Maps
{
    public class PortalController : MonoBehaviour
    {
        private const float _spawnAnimLength = 1.5f;
        private const float _spawnAnimEventDelay = 0.5f;


        private const float _closeAnimLength = 1f;
        private const float _closeAnimEventDelay = 0.75f;

        public event Action OnPortalOpen;
        public event Action OnPortalClose;

        [SerializeField] private Animator[] _portalAnimators;

        [SerializeField] private AudioSource? _audioSource;
        [SerializeField] private AudioClip? _portalOpenClip;
        [SerializeField] private AudioClip? _portalCloseClip;

        public async UniTask OpenAsync()
        {
            if (_audioSource != null && _portalOpenClip != null)
            {
                _audioSource.PlayOneShot(_portalOpenClip);
            }

            foreach (var animator in _portalAnimators)
            {
                animator.gameObject.SetActive(true);
                animator.Play("OpenPortal");
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_spawnAnimEventDelay));

            OnPortalOpen?.Invoke();
        }

        public async UniTask CloseAsync()
        {
            if (_audioSource != null && _portalCloseClip != null)
            {
                _audioSource.PlayOneShot(_portalCloseClip);
            }

            foreach (var animator in _portalAnimators)
            {
                animator.Play("ClosePortal");
            }

            await UniTask.Delay(TimeSpan.FromSeconds(_closeAnimEventDelay));

            OnPortalClose?.Invoke();
        }
    }
}
