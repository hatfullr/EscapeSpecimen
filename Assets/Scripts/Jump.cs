using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour
{
    [SerializeField, Min(0f)] private float duration = 1f;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Quaternion startRotation;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Quaternion endRotation;

    [SerializeField] private UnityEvent onBegin;

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
        Vector3 position = Vector3.Lerp(startPosition, endPosition, progress);
        Quaternion rotation = Quaternion.Lerp(startRotation, endRotation, progress);
        transform.SetPositionAndRotation(position, rotation);
    }

    public void Begin()
    {
        active = true;
        onBegin.Invoke();
    }
}
