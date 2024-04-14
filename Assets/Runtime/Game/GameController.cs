using System;
using SoldByWizards.Maps;
using UnityEngine;

namespace SoldByWizards.Game
{
    // Each "day" is one teleportation
    public class GameController : MonoBehaviour
    {
        [SerializeField] private float _baseRentAmount = 50f;
        [SerializeField] private float _rentIncreasePerCycle = 25f;         // TODO: make into curve
        [SerializeField] private float _secondsPerRentCycle = 60f;

        private int _currentDay = 0;
        private float _currentMoney;
        private float _timeElapsed = 0f;
        private bool _dayIsProgressing = false;

        private void Start()
        {
            // Start at day 0
            _dayIsProgressing = true;
        }

        private void Update()
        {
            if (!_dayIsProgressing)
                return;

            _timeElapsed += Time.deltaTime;

            if (_timeElapsed > _secondsPerRentCycle)
            {
                _dayIsProgressing = false;
                CheckForNextDay();
            }
        }

        private void CheckForNextDay()
        {
            var rent = GetRentForCurrentDay();
            if (_currentMoney < rent)
            {
                // fail player, game over
                Debug.LogError("YOU FREAKIN' DIED!");
            }
            else
            {
                // progress to next day
                _timeElapsed = 0f;
                _dayIsProgressing = true;
                _currentDay += 1;
            }
        }

        private float GetRentForCurrentDay()
        {
            return _baseRentAmount + (_currentDay * _rentIncreasePerCycle);
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }
    }
}
