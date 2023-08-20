using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Snappable : MonoBehaviour
{
    private RectTransform _rectTransform;
    [HideInInspector] public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null) _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }


    // Given the pointer data, return the global position to snap to for this snappable
    // The default behavior (below) is to return the center of this GameObject
    // This function returns false if the snapping is not allowed.
    public virtual Vector3 Snap(Transform dragObj)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        return 0.5f * (corners[2] + corners[0]);
    }

    public virtual bool CanSnap(Transform dragObj)
    {
        return true;
    }
}