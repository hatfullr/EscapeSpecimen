using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOrderManager : MonoBehaviour
{
    private Canvas _canvas;
    [HideInInspector] public Canvas canvas
    {
        get
        {
            if (_canvas == null) _canvas = GetComponentInParent<Canvas>();
            return _canvas;
        }
    }

    public void SendToTop()
    {
        transform.SetParent(canvas.transform.GetChild(canvas.transform.childCount - 1));
    }
}
