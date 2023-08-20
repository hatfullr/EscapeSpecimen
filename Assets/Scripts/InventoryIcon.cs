using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable), typeof(Image))]
public class InventoryIcon : MonoBehaviour
{
    [SerializeField] private InventoryIconEnum _type;

    [HideInInspector] public InventoryIconEnum type { get => _type; }

    private Draggable _draggable;
    [HideInInspector] public Draggable draggable
    {
        get
        {
            if (_draggable == null) _draggable = GetComponent<Draggable>();
            return _draggable;
        }
    }

    private Image _image;
    [HideInInspector] public Image image
    {
        get
        {
            if (_image == null) _image = GetComponent<Image>();
            return _image;
        }
    }

    [HideInInspector] public InventorySlot slot;


    public void OnDragFinished()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;
        Snappable snappable = draggable.GetTopSnappable(data);
        if (snappable == null)
        {
            SendBackToSlot();
        }
    }

    public void SendBackToSlot()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = slot.transform.position;
        draggable.SetDraggedPosition(data);
    }
}
