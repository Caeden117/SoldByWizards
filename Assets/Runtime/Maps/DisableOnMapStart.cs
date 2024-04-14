using UnityEngine;

namespace SoldByWizards.Maps
{
    public class DisableOnMapStart : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private TimedMapLoader _timedMapLoader;

        private void Start()
        {
            _timedMapLoader.OnTimerStarted += OnTimerStarted;
            _timedMapLoader.OnTimerEnded += OnTimerEnded;
        }

        private void OnTimerEnded() => _target.SetActive(true);

        private void OnTimerStarted() => _target.SetActive(false);

        private void OnDestroy()
        {
            _timedMapLoader.OnTimerStarted -= OnTimerStarted;
            _timedMapLoader.OnTimerEnded -= OnTimerEnded;
        }
    }
}
