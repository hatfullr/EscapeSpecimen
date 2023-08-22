using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private float duration = 2;

    private float timer = 0f;

    public UnityEvent onFinished;

    private void Update()
    {
        timer = Mathf.Min(timer + Time.deltaTime, duration);
        if (timer >= duration)
        {
            onFinished.Invoke();
        }
    }
}
