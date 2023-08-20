using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<InventorySlot> _slots;
    [HideInInspector] public List<InventorySlot> slots
    {
        get
        {
            if (_slots == null)
            {
                _slots = new List<InventorySlot>(GetComponentsInChildren<InventorySlot>(true));
                //_slots.Reverse();
            }
            return _slots;
        }
    }

    public void AddToInventory(InventoryItem item)
    {
        bool success = false;
        foreach (InventorySlot slot in slots)
        {
            if (!slot.occupied)
            {
                slot.Occupy(item.icon);
                success = true;
                break;
            }
        }

        if (!success) throw new System.NotImplementedException("Help");

        Destroy(item.gameObject);
    }
}
