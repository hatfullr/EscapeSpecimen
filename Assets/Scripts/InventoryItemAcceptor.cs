using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItemAcceptor : MonoBehaviour
{
    [SerializeField] private InventoryIconEnum acceptedTypes;

    [SerializeField] private UnityEvent onItemAccepted;
    
    

    public void AcceptItem(InventoryIcon icon)
    {
        Debug.Log("Accept Item");
        Destroy(icon.gameObject);
        onItemAccepted.Invoke();
    }

    public bool CanAccept(InventoryIcon icon)
    {
        return acceptedTypes.HasFlag(icon.type);
    }
}
