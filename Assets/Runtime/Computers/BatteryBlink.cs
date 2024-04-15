using UnityEngine;

namespace SoldByWizards.Computers
{
    public class BatteryBlink : MonoBehaviour
    {
        public GameObject? Battery;
        public float BlinkDuration;

        private float _timer;
        private bool _activeStatus = false;

        private void Update()
        {
            if (Battery == null)
                return;

            _timer += Time.deltaTime;

            if (_timer > BlinkDuration)
            {
                _timer = 0;
                _activeStatus = !_activeStatus;
                Battery.SetActive(_activeStatus);
            }
        }
    }
}
