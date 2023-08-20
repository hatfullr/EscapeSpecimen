using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceMainCamera : MonoBehaviour
{
    [SerializeField] private bool billboard = true;
    [SerializeField] private bool faceAway = false;


    // Update is called once per frame
    void Update()
    {
        if (Camera.main != null)
        {
            if (billboard)
            {
                Vector3 direction = Camera.main.transform.forward;
                if (faceAway) direction = -direction;
                transform.rotation = Quaternion.LookRotation(direction);
            }
            else transform.LookAt(Camera.main.transform);
        }
    }
}
