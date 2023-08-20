using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItemAcceptor : MonoBehaviour
{
    [SerializeField] private InventoryIconEnum acceptedTypes;
    [SerializeField] private bool destroyOnAccept = true;


    [SerializeField] private UnityEvent onItemAccepted;


    private void Start() { }

    public void AcceptItem(InventoryIcon icon)
    {
        if (destroyOnAccept) Destroy(icon.gameObject);
        onItemAccepted.Invoke();
    }

    public bool CanAccept(InventoryIcon icon)
    {
        return enabled && acceptedTypes.HasFlag(icon.type);
    }
}
