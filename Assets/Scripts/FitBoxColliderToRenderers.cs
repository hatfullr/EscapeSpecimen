using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(BoxCollider))]
public class FitBoxColliderToRenderers : MonoBehaviour
{
    [SerializeField] private Transform _transformToFit;
    [SerializeField] private Alignment alignment = Alignment.Center;
    [SerializeField, Min(0f)] private Vector3 minimumScale = Vector3.zero;

    [HideInInspector] public Transform transformToFit { get => _transformToFit; }

    private BoxCollider _boxCollider;
    private BoxCollider boxCollider
    {
        get
        {
            if (_boxCollider == null) _boxCollider = GetComponent<BoxCollider>();
            return _boxCollider;
        }
    }

    private List<Renderer> _renderers;
    private List<Renderer> renderers
    {
        get
        {
            if (_renderers == null)
            {
                if (transformToFit == null)
                    _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());
                else _renderers = new List<Renderer>(transformToFit.GetComponentsInChildren<Renderer>());
            }
            return _renderers;
        }
    }

    private enum Alignment
    {
        Center,
        Bottom,
        Top,
        Right,
        Left
    }


    void Update()
    {
        if (Application.isPlaying) return;
        if (renderers.Count > 0)
        {
            Bounds bounds = GetBounds();
            bounds = ApplyMinimumScale(bounds);
            bounds = ApplyAlignment(bounds);
            boxCollider.center = bounds.center;
            boxCollider.size = bounds.size;
        }
    }

    private Bounds ApplyMinimumScale(Bounds bounds)
    {
        Vector3 size = bounds.size;
        bounds.size = Vector3.Max(size, minimumScale);
        return bounds;
    }


    private Bounds GetBounds()
    {
        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;
        foreach (Renderer renderer in renderers)
        {
            min = Vector3.Min(min, Vector3.Min(renderer.bounds.min, renderer.bounds.max));
            max = Vector3.Max(max, Vector3.Max(renderer.bounds.min, renderer.bounds.max));
        }

        min -= transform.position;
        max -= transform.position;

        Vector3 inverseLocalScale = new Vector3(1f / transform.lossyScale.x, 1f / transform.lossyScale.y, 1f / transform.lossyScale.z);

        min.x *= inverseLocalScale.x;
        min.y *= inverseLocalScale.y;
        min.z *= inverseLocalScale.z;
        max.x *= inverseLocalScale.x;
        max.y *= inverseLocalScale.y;
        max.z *= inverseLocalScale.z;

        if (transform.lossyScale.x == 0)
        {
            min.x = 0f;
            max.x = 0f;
        }
        if (transform.lossyScale.y == 0)
        {
            min.y = 0f;
            max.y = 0f;
        }
        if (transform.lossyScale.z == 0)
        {
            min.z = 0f;
            max.z = 0f;
        }
        
        return new Bounds(0.5f * (min + max), max - min);
    }

    /// <summary>
    /// Align the given bounds
    /// </summary>
    private Bounds ApplyAlignment(Bounds bounds)
    {
        Vector3 center = bounds.center;
        Vector3 halfExtents = 0.5f * bounds.size;
        if (alignment == Alignment.Top) center -= Vector3.up * Vector3.Dot(Vector3.up, halfExtents);
        else if (alignment == Alignment.Bottom) center -= Vector3.down * Vector3.Dot(Vector3.up, halfExtents);
        else if (alignment == Alignment.Right) center -= Vector3.right * Vector3.Dot(Vector3.right, halfExtents);
        else if (alignment == Alignment.Left) center -= Vector3.left * Vector3.Dot(Vector3.right, halfExtents);
        bounds.center = center;
        return bounds;
    }
}
