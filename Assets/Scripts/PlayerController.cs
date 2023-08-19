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
        position.currentViewingAngle.moveForward.MovePlayerToThisPosition(this);
    }

    private void MoveBackwards()
    {
        Debug.Log("Move Backwards", gameObject);
    }

    private void TurnLeft()
    {
        Debug.Log("Turn Left", gameObject);
    }

    private void TurnRight()
    {
        Debug.Log("Turn Right", gameObject);
    }



    public void SetPosition(PlayerPosition position)
    {
        this.position = position;
        // Change this later when we want to preserver player direction on move
        transform.SetPositionAndRotation(transform.position, position.firstView.transform.rotation);
        //controller.position.player = null;
        //player = controller;
    }
}
