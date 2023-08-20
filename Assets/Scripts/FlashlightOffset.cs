using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOffset : MonoBehaviour
{
    [SerializeField] private float amount = 0.1f;

    private PlayerController _player;
    [HideInInspector] public PlayerController player
    {
        get
        {
            if (_player == null) _player = GetComponentInParent<PlayerController>(true);
            if (_player == null) throw new System.Exception("Failed to find a PlayerController in the parents of a FlashlightOffset "+gameObject);
            return _player;
        }
    }

    private Flashlight _flashlight;
    [HideInInspector] public Flashlight flashlight
    {
        get
        {
            if (_flashlight == null) _flashlight = player.GetComponentInChildren<Flashlight>(true);
            if (_flashlight == null) throw new System.Exception("The player has no Flashlight object");
            return _flashlight;
        }
    }

    private Vector2 originalScreenPosition;

    private Vector3 previousMousePosition;


    void Update()
    {
        SetPosition();

        previousMousePosition = Input.mousePosition;
    }

    private void SetPosition()
    {
        //Vector2 offset = (Input.mousePosition - previousMousePosition) * amount;
        Vector3 world1 = flashlight.ScreenToWorldPosition(Input.mousePosition);
        Vector3 world2 = flashlight.ScreenToWorldPosition(previousMousePosition);
        Vector3 offset = world1 - world2;
        transform.position += offset;
    }
}
