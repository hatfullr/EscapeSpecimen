using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Draggable), typeof(Image))]
public class InventoryIcon : MonoBehaviour
{
   

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
}
