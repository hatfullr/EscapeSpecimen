using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Draggable : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public bool centered = false;
    public UnityEvent onDragStarted = new UnityEvent();
    public UnityEvent onDragFinished = new UnityEvent();

    [Header("Debugging")]
    [ReadOnly] public bool pickedUp = false;
    [ReadOnly] public Vector3 globalMousePos;

    //[HideInInspector] public RectTransform draggingPlane;

    private RectTransform _rectTransform;
    private RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    private Vector3 previousMousePos;
    private bool firstFramePickedUp = false;



    void Update()
    {
        if (!enabled) return;
        if (pickedUp)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            SetDraggedPosition(data);

            if (!firstFramePickedUp && Input.GetMouseButtonUp(0)) SetDown();
            firstFramePickedUp = false;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
                {
                    PointerEventData data = new PointerEventData(EventSystem.current);
                    data.position = Input.mousePosition;
                    PickUp(data);
                }
            }
        }
    }

    public void PickUp(PointerEventData data = null)
    {
        if (!enabled) return;
        if (data == null)
        {
            data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
        }

        StartDrag(data);
        pickedUp = true;
        firstFramePickedUp = true;
    }

    public void SetDown()
    {
        if (!enabled) return;

        InventoryIcon icon = GetComponent<InventoryIcon>();
        if (icon != null)
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;

            Draggable draggable = GetTopDraggable(data);
            if (draggable != null)
            {
                Image image = draggable.GetComponent<Image>();
                if (image != null)
                {
                    image.raycastTarget = true;
                }
            }

            Snappable snappable = GetTopSnappable(data);
            if (snappable != null)
            {
                InventorySlot slot = snappable.GetComponent<InventorySlot>();
                if (slot != null)
                {
                    if (slot.occupied && icon.slot != null) icon.SendBackToSlot();
                    else slot.Occupy(icon);
                }
            }
            else
            {
                InventoryItemAcceptor acceptor = GetTopAcceptor(data);
                if (acceptor != null)
                {
                    if (acceptor.CanAccept(icon)) acceptor.AcceptItem(icon);
                }
            }
        }

        pickedUp = false;
        onDragFinished.Invoke();
    }

    public void StartDrag(PointerEventData data)
    {
        if (!enabled) return;
        GetGlobalMousePos(data);
        onDragStarted.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (!enabled) return;

        // Only release the object on mouse pointer up if the object was attached to the mouse by some other script
        if (pickedUp) return;

        // Make sure it was the left mouse button which was used
        if (Input.GetMouseButtonDown(0))
        {
            // If there's any draggables on top of this one, then drag that instead.
            Draggable draggable = GetTopDraggable(data);
            draggable.StartDrag(data);

            Image image = draggable.GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = false;
            }
        }
    }

    public virtual void OnDrag(PointerEventData data)
    {
        if (!enabled) return;
        // Make sure it was the left mouse button which was used
        if (Input.GetMouseButton(0))
        {
            pickedUp = false;
            SetDraggedPosition(data);
        }
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        if (!enabled) return;
        //Debug.Log("Pointer Up");
        // Make sure it was the left mouse button which was used
        if (Input.GetMouseButtonUp(0)) SetDown();
    }

    public virtual void SetDraggedPosition(PointerEventData data)
    {
        if (GetGlobalMousePos(data))
        {
            if (centered) rectTransform.position = (Vector2)globalMousePos;
            else rectTransform.position = (Vector2)rectTransform.position + (Vector2)(globalMousePos - previousMousePos);
        }
    }

    public bool GetGlobalMousePos(PointerEventData data)
    {
        previousMousePos = globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out globalMousePos))
        {
            // Snap to center of Snappable if there is one
            Snappable snappable = GetTopSnappable(data);
            if (snappable != null)
            {
                globalMousePos = snappable.Snap(rectTransform);
                transform.SetParent(snappable.transform, true);
            }
            else if (transform.parent.GetComponent<Snappable>() != null)
            {
                GetComponent<UIOrderManager>().SendToTop();
            }
            return true;
        }
        return false;
    }

    public Snappable GetTopSnappable(PointerEventData data)
    {
        // Do a raycast through the mouse position to check for any Snappable objects
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, hits);
        foreach (RaycastResult hit in hits)
        {
            Snappable snappable = hit.gameObject.GetComponent<Snappable>();
            if (snappable != null)
                if (snappable.CanSnap(rectTransform))
                    return snappable;
        }
        return null;
    }

    public Draggable GetTopDraggable(PointerEventData data)
    {
        // Do a raycast through the mouse position to check for any Draggable objects
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, hits);
        foreach (RaycastResult hit in hits)
        {
            Draggable draggable = hit.gameObject.GetComponent<Draggable>();
            if (draggable != null) return draggable;
        }
        return null;
    }

    public InventoryItemAcceptor GetTopAcceptor(PointerEventData data)
    {
        // Do a raycast through the mouse position to check for any Snappable objects
        Ray ray = Camera.main.ScreenPointToRay(data.position);
        List<RaycastResult> hits = new List<RaycastResult>();
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            InventoryItemAcceptor acceptor = hit.collider.gameObject.GetComponent<InventoryItemAcceptor>();
            if (acceptor != null) return acceptor;
        }
        return null;
    }
}
