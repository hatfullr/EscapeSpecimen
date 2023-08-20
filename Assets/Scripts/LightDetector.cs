using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Detect the intensity of light shining on the location of this GameObject.
/// </summary>
public class LightDetector : MonoBehaviour
{
    [Tooltip("The function of intensity over distance for every light in the scene. The 'time' axis is the dimensionless distance and " +
        "the other axis is the fraction of each light's intensity.")]
    [SerializeField] private AnimationCurve kernel = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private AnimationCurve spotLightKernel = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    [Header("Debugging")]
#pragma warning disable IDE0052 // Remove unread private members
    [ReadOnly, SerializeField] private int nlights;
#pragma warning restore IDE0052 // Remove unread private members
    [ReadOnly] public float intensity;
    [SerializeField] private bool showLights;

    private List<Light> lights;
    private List<Light> contributingLights = new List<Light>();


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        // Your gizmo drawing thing goes here if required...
        if (showLights)
        {
            Gizmos.color = Color.white;
            foreach (Light light in contributingLights)
            {
                Gizmos.DrawLine(transform.position, light.transform.position);
            }
        }

        // Ensure continuous Update calls.
        if (!Application.isPlaying)
        {
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
        }
    }
#endif


    void Update()
    {
        lights = new List<Light>(FindObjectsOfType<Light>(true));
        contributingLights.Clear();

        nlights = lights.Count;
        float distance;
        Vector3 diff;
        intensity = 0f;
        foreach (Light light in lights)
        {
            if (light.enabled && (light.type == LightType.Point || light.type == LightType.Spot))
            {
                diff = light.transform.position - transform.position; // direction of light from this (target - origin)
                distance = diff.magnitude;
                if (distance < light.range)
                {
                    // Check for obstacles between
                    if (!Physics.Raycast(light.transform.position, diff.normalized, distance, gameObject.layer))
                    {
                        float newIntensity = 0f;
                        if (light.type == LightType.Point)
                            newIntensity = light.intensity / (distance * distance) * kernel.Evaluate(distance / light.range);
                        else if (light.type == LightType.Spot)
                        {
                            Vector3 direction = (transform.position - light.transform.position).normalized; // direction of us from light (target - origin)
                            float dot = Vector3.Dot(direction, light.transform.forward);
                            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                            if (angle < 0.5f * light.spotAngle)
                                newIntensity = light.intensity * kernel.Evaluate(distance / light.range);
                            float spotAngle = 0.5f * light.spotAngle;
                            float angleClamped = Mathf.Clamp(angle, 0f, spotAngle);
                            float angleFrac = angleClamped / spotAngle;
                            newIntensity *= spotLightKernel.Evaluate(angleFrac);
                        }
                        intensity = Mathf.Max(intensity, newIntensity);
                        contributingLights.Add(light);
                    }
                }
            }
        }
    }

    public float GetMaximumIntensity()
    {
        float result = -float.PositiveInfinity;
        foreach (Light light in contributingLights)
        {
            result = Mathf.Max(result, light.intensity);
        }
        return result;
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(LightDetector))]
public class LightDetectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Currently this script only works with Point lights and Spot lights that have their inner angle set to be the same as their outer angle.", MessageType.Warning);
        base.OnInspectorGUI();
    }
}
#endif