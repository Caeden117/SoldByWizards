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
        public event Action<List<Item>>? OnItemsCollected;
        public event Action<List<Item>>? OnItemsListedForSale;
        public event Action<Computer>? OnComputerSelected;
        public event Action<Computer>? OnComputerDeselected;
        public event Action OnSpamTypingFinished;

        public void RegisterCollectedItems(List<Item> items)
        {
            OnItemsCollected?.Invoke(items);
        }

        public void ListItemsForSale(List<Item> items)
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

        public void FinishSpamTyping()
        {
            OnSpamTypingFinished?.Invoke();
        }
    }
}
