using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private InventoryIcon _icon;
    [HideInInspector] public InventoryIcon icon
    {
        get
        {
            if (_icon == null) _icon = GetComponentInChildren<InventoryIcon>(true);
            return _icon;
        }
    }

    private Inventory _inventory;
    [HideInInspector] public Inventory inventory
    {
        get
        {
            if (_inventory == null) _inventory = FindObjectOfType<Inventory>(true);
            return _inventory;
        }
    }

    public void SendToInventory()
    {
        inventory.AddToInventory(this);
        icon.gameObject.SetActive(true);
    }
}
