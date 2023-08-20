using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Snappable))]
public class InventorySlot : MonoBehaviour
{
    [SerializeField, ReadOnly] private bool _occupied = false;

    [HideInInspector] public bool occupied { get => _occupied; private set => _occupied = value; }

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
        occupied = true;

        icon.transform.SetParent(transform, false);
        //icon.transform.localPosition = transform.localPosition;
    }

}
