using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour
{
    [SerializeField, Min(0f)] private float duration = 1f;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 startRotation;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 endRotation;

    [SerializeField] private UnityEvent onBegin;

    private bool reversed = false;

    private float timer = 0f;

    private bool active = false;

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            timer = Mathf.Min(timer + Time.deltaTime, duration);
            if (timer == duration) active = false;

            UpdatePosition();
        }
    }


    private void UpdatePosition()
    {
        float progress = timer / duration;
        Vector3 position;
        Quaternion rotation;
        if (reversed)
        {
            position = Vector3.Lerp(endPosition, startPosition, progress);
            rotation = Quaternion.Euler(Vector3.Lerp(endRotation, startRotation, progress));
        }
        else
        {
            position = Vector3.Lerp(startPosition, endPosition, progress);
            rotation = Quaternion.Euler(Vector3.Lerp(startRotation, endRotation, progress));
        }
        transform.SetPositionAndRotation(position, rotation);
    }

    public void Begin()
    {
        timer = 0f;
        reversed = false;
        active = true;
        onBegin.Invoke();
    }

    public void BeginReversed()
    {
        timer = 0f;
        reversed = true;
        active = true;
        onBegin.Invoke();
    }
}
