using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LightController))]
public class LightFlicker : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float strength = 0.1f;
    [SerializeField, Min(0f)] private float minDuration = 0.1f;
    [SerializeField, Min(0f)] private float maxDuration = 1f;
    [Tooltip("Prevents the light's intensity from falling below this value.")]
    [SerializeField] private float minIntensity = 0.001f;

    [Header("Debugging")]
    [ReadOnly, SerializeField] private float modifier;


    private bool _flickering;

    private LightController _lightSource;
    private LightController lightSource
    {
        get
        {
            if (_lightSource == null) _lightSource = GetComponent<LightController>();
            return _lightSource;
        }
    }

    private float timer;
    private float timeLimit;
    

    void Update()
    {
        UpdateFlicker();
        timer = Mathf.Min(timer + Time.deltaTime, timeLimit);
        if (timer == timeLimit) StartFlicker();
    }


    public void Start()
    {
        StartFlicker();
    }

    private void StartFlicker()
    {
        timer = 0f;
        timeLimit = Random.Range(minDuration, maxDuration);
        float delta = lightSource.intensity * strength;
        modifier = 0.5f * Random.Range(-delta, delta);
    }
    
    private void UpdateFlicker()
    {
        lightSource.modifier = Mathf.Lerp(lightSource.modifier, modifier, timer / timeLimit);

        // The light source's intensity will be this value next frame
        float intensity = lightSource.intensity + lightSource.modifier;
        // Prevent the light's intensity from going below this value
        if (intensity < minIntensity)
        {
            lightSource.modifier = 0f;
        }
    }
}