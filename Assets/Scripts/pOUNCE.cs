using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class pOUNCE : MonoBehaviour
{
    [SerializeField] private GameObject spider;
    [SerializeField] private float cameraDistance = 10f;
    [SerializeField] private float duration = 1f;

    private float timer = 0f;
    private bool active = false;

    private Vector3 startposition;

    [SerializeField] private UnityEvent onFinished;

    public void Begin()
    {
        transform.position = Camera.main.transform.position + Camera.main.transform.forward * cameraDistance;
        timer = 0f;
        active = true;
        
        //transform.rotation = Quaternion.Euler(Camera.main.transform.right);
        startposition = transform.position;
    }

    void Update()
    {
        if (active)
        {
            Vector3 direction = (Camera.main.transform.position - transform.position).normalized;
            spider.transform.rotation = Quaternion.Euler(direction);
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startposition, Camera.main.transform.position, Mathf.Clamp(timer / duration, 0f, 1f));
            if (timer >= duration)
            {
                onFinished.Invoke();
                active = false;
            }
        }
        
    }
}
