using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D normalCursor;
    [SerializeField] private Texture2D hoverCursor;


    void Update()
    {
        if (HoveringOnInteraction()) Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
        else Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
    }

    private bool HoveringOnInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.GetComponent<Interaction>() != null)
            {
                return true;
            }
        }
        return false;
    }
}
