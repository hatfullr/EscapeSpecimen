using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Snappable))]
public class InventorySlot : MonoBehaviour
{
    [ReadOnly] public bool occupied = false;

    private Snappable _snappable;
    [HideInInspector] public Snappable snappable
    {
        get
        {
            if (_snappable == null) _snappable = GetComponent<Snappable>();
            return _snappable;
        }
    }


    public void Occupy(InventoryIcon icon)
    {
        Debug.Log("Occupy " + icon.slot,gameObject);
        if (icon.slot != null) icon.slot.occupied = false;
        icon.transform.SetParent(transform, false);
        icon.slot = this;
        occupied = true;
    }

}
