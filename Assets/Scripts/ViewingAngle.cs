using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewingAngle : MonoBehaviour
{
    [SerializeField] private PlayerPosition _moveForward;

    [HideInInspector] public PlayerPosition moveForward { get => _moveForward; }


    public void SetPlayerView(PlayerController controller)
    {
        controller.transform.rotation = transform.rotation;
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
