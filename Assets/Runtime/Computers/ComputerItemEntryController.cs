using System.Collections.Generic;
using SoldByWizards.Items;
using UnityEngine;

namespace SoldByWizards.Computers
{
    public class ComputerItemEntryController : MonoBehaviour
    {
        // TODO: Hook up dynamically
        [SerializeField] public List<ItemSO> CurrentItems = new();
    }
}
