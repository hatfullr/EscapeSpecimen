using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingAngle : MonoBehaviour
{
    public PlayerPosition moveForward;


    private PlayerPosition _playerPosition;
    [HideInInspector] public PlayerPosition playerPosition
    {
        get
        {
            if (_playerPosition == null) _playerPosition = GetComponentInParent<PlayerPosition>(true);
            if (_playerPosition == null) throw new System.Exception("ViewingAngle is not a child of a PlayerPosition");
            return _playerPosition;
        }
    }

    public void SetMoveForward(PlayerPosition position)
    {
        moveForward = position;
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (moveForward != null)
        {
            Gizmos.DrawLine(transform.position, moveForward.transform.position);
        }
    }
#endif
}
