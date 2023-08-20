using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverTimer : MonoBehaviour
{
    [SerializeField] private float duration;

    private float timer;

    [SerializeField] private UnityEvent onStart;
    [SerializeField] private UnityEvent onStop;

    private void Update()
    {
        timer = Mathf.Min(timer + Time.deltaTime, duration);
        if (timer == duration) 
        {
            onStop.Invoke();
            enabled = false;
        }
    }


    public void Begin()
    {
        timer = 0f;
        onStart.Invoke();
    }
}
