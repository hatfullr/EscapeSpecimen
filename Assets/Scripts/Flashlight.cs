using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    private Camera _camera;
    [HideInInspector] public new Camera camera
    {
        get
        {
            if(_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    void LateUpdate()
    {
        SetRotation();
    }

    private void SetRotation()
    {
        transform.LookAt(GetMouseWorldPosition());
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screen = Input.mousePosition;
        screen.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(screen);
    }

}
