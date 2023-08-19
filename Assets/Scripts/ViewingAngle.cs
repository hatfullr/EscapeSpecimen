using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingAngle : MonoBehaviour
{
    [SerializeField] private PlayerPosition _moveForward;

    [HideInInspector] public PlayerPosition moveForward { get => _moveForward; }


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
