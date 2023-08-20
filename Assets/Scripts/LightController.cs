using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Light)), ExecuteInEditMode]
public class LightController : MonoBehaviour
{
    [Tooltip("If true, Activate is called when this script is enabled, but only if the Light is also enabled.")]
    [SerializeField] private bool activateOnEnable = true;
    [SerializeField] private AnimationCurve activationEffect = AnimationCurve.EaseInOut(0f, 0f, 1f, 10f);
    [SerializeField] private AnimationCurve deactivationEffect = AnimationCurve.EaseInOut(0f, 10f, 1f, 0f);

    public UnityEvent activateLight;
    public UnityEvent deactivateLight;

    private Light _light;
    [HideInInspector] public new Light light
    {
        get
        {
            if (_light == null) _light = GetComponent<Light>();
            return _light;
        }
    }

    private float timer = 0f;
    private bool activating, deactivating;
    private float duration = 0f;

    //private Timer activateTimer;
    //private Timer deactivateTimer;

    [HideInInspector] public float intensity;
    [HideInInspector] public float modifier;


    void Update()
    {
        if (activating)
        {
            intensity = activationEffect.Evaluate(timer);
        }
        else if (deactivating)
        {
            intensity = deactivationEffect.Evaluate(timer);
        }

        if (activating || deactivating)
        {
            timer = Mathf.Min(timer + Time.deltaTime, duration);
            if (timer == duration)
            {
                if (activating) intensity = activationEffect.Evaluate(duration);
                else
                {
                    intensity = deactivationEffect.Evaluate(duration);
                    light.enabled = false;
                }
                timer = 0f;
                activating = false;
                deactivating = false;
            }
        }
        light.intensity = intensity + modifier;
    }

    void OnEnable()
    {
        if (activateOnEnable && light.enabled) Activate();
    }

    void Reset()
    {
        timer = 0f;
        activating = false;
        deactivating = false;
    }

    public void Activate()
    {
        if (activationEffect.keys.Length > 0)
        {
            /*
            if (Application.isEditor && !Application.isPlaying)
            {
                if (activateTimer != null) DestroyImmediate(activateTimer.gameObject);
                if (deactivateTimer != null) DestroyImmediate(deactivateTimer.gameObject);
            }
            else
            {
                if (activateTimer != null) Destroy(activateTimer.gameObject);
                if (deactivateTimer != null) Destroy(deactivateTimer.gameObject);
            }
            */
            duration = activationEffect.keys[activationEffect.keys.Length - 1].time;
            timer = 0f;
            activating = true;
            deactivating = false;
            light.enabled = true;
            //activateTimer = TimerManager.CreateTimer(t1, "Activation", transform, true, true);
            //activateTimer.AddOnStart(() => light.enabled = true);
            //activateTimer.AddOnUpdate(() => intensity = activationEffect.Evaluate(activateTimer.time));
            activateLight.Invoke();
        }
    }

    public void DeActivate()
    {
        if (deactivationEffect.keys.Length > 0)
        {
            /*
            if (deactivateTimer != null) Destroy(deactivateTimer.gameObject);
            if (activateTimer != null) Destroy(activateTimer.gameObject);
            */
            duration = deactivationEffect.keys[deactivationEffect.keys.Length - 1].time;
            timer = 0f;
            activating = false;
            deactivating = true;
            //deactivateTimer = TimerManager.CreateTimer(t1, "Deactivation", transform, true, true);
            //deactivateTimer.AddOnStop(() => light.enabled = false);
            //deactivateTimer.AddOnUpdate(() => intensity = deactivationEffect.Evaluate(deactivateTimer.time));
            deactivateLight.Invoke();
        }
    }

    public void SetIntensity(float value) { intensity = value; }
}



#if UNITY_EDITOR
[CustomEditor(typeof(LightController)), CanEditMultipleObjects]
public class LightControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Activate")) (target as LightController).Activate();
        if (GUILayout.Button("Deactivate")) (target as LightController).DeActivate();
    }
}
#endif