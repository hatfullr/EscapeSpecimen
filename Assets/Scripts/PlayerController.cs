using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    

    [Header("Debugging")]
    [ReadOnly] public PlayerPosition position;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) MoveForward();
        if (Input.GetKeyDown(KeyCode.S)) MoveBackwards();
        if (Input.GetKeyDown(KeyCode.A)) TurnLeft();
        if (Input.GetKeyDown(KeyCode.D)) TurnRight();
    }

    private void MoveForward()
    {
        Debug.Log("Move Forward", gameObject);
        SetPosition(position.currentViewingAngle.moveForward);
    }

    private void MoveBackwards()
    {
        Debug.Log("Move Backwards", gameObject);
    }

    private void TurnLeft()
    {
        Debug.Log("Turn Left", gameObject);
        position.TurnLeft(this);
    }

    private void TurnRight()
    {
        Debug.Log("Turn Right", gameObject);
        position.TurnRight(this);
    }



    public void SetPosition(PlayerPosition position)
    {
        this.position = position;
        position.player = this;

        transform.position = position.transform.position;

        // Change this later when we want to preserve player direction on move
        SetView(position.firstView);
    }

    public void SetView(ViewingAngle view)
    {
        view.playerPosition.ChangeView(this, view);
    }
}
