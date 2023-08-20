using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    private Camera _camera;
    [HideInInspector] public new Camera camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    private Light _light;
    [HideInInspector] public new Light light
    {
        get
        {
            if (_light == null) _light = GetComponent<Light>();
            return _light;
        }
    }

    void LateUpdate()
    {
        SetRotation();
        if (Input.GetKeyDown(KeyCode.Space)) Toggle();
    }

    private void SetRotation()
    {
        transform.LookAt(GetMouseWorldPosition());
    }

    public Vector3 GetMouseWorldPosition()
    {
        return ScreenToWorldPosition(Input.mousePosition);
    }

    public Vector3 ScreenToWorldPosition(Vector3 screen)
    {
        screen.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(screen);
    }

    public Vector2 WorldToScreenPosition(Vector3 world)
    {
        return camera.WorldToScreenPoint(world);
    }

    public void Toggle()
    {
        light.enabled = !light.enabled;
    }

}
