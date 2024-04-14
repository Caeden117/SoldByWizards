using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Computers
{
    // used for external events and hooking things up
    public class ComputerController : MonoBehaviour
    {
        [SerializeField]
        private List<ItemSO> _debugItems = new();

        public event Action<List<ItemSO>>? OnItemsCollected;
        public event Action<List<ItemSO>>? OnItemsListedForSale;
        public event Action<Computer>? OnComputerSelected;
        public event Action<Computer>? OnComputerDeselected;

        public void RegisterCollectedItems(List<ItemSO> items)
        {
            OnItemsCollected?.Invoke(items);
        }

        public void ListItemsForSale(List<ItemSO> items)
        {
            OnItemsListedForSale?.Invoke(items);
        }

        public void ComputerSelected(Computer computer)
        {
            OnComputerSelected?.Invoke(computer);
        }

        public void ComputerDeselected(Computer computer)
        {
            OnComputerDeselected?.Invoke(computer);
        }

        public async void Start()
        {
            await UniTask.Delay(5000);
            RegisterCollectedItems(_debugItems);
        }
    }
}
