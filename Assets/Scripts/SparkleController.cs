using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkleController : MonoBehaviour
{
    [SerializeField, Min(0f)] private float minimumIntensity = 0.1f;

    [SerializeField] private LightDetector lightDetector;
    private Dictionary<ParticleSystem, float> particleSystems = new Dictionary<ParticleSystem, float>();

    void Awake()
    {
        foreach (ParticleSystem particleSystem in GetComponentsInChildren<ParticleSystem>(true))
        {
            particleSystems.Add(particleSystem, particleSystem.main.startSize.constant);
        }
    }

    void Start()
    {
        SetIntensity(0f);
    }

    void LateUpdate()
    {
        if (lightDetector != null) SetIntensity(lightDetector.intensity / lightDetector.GetMaximumIntensity());
    }

    public void SetIntensity(float value)
    {
        value = Mathf.Max(value, minimumIntensity);

        foreach (ParticleSystem particleSystem in particleSystems.Keys)
        {
            var module = particleSystem.main;
            var rate = module.startSize;
            rate.constant = particleSystems[particleSystem] * value;
            module.startSize = rate;
        }
    }
}
