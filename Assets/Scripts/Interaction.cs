using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Interaction : MonoBehaviour
{
    [Tooltip("If single is true, then only 1 interaction is allowed in this object's lifetime.")]
    [SerializeField] private bool _single;

    public UnityEvent interaction = new UnityEvent();
    public UnityEvent interactionFailed = new UnityEvent();

    [Header("Debugging")]
    [ReadOnly, SerializeField] private bool _interacted = false;
    [Tooltip("When this is true, the mouse will show an icon next to it when hovered over this GameObject.")]
    [ReadOnly, SerializeField] private bool _interactionShown = true;


    [HideInInspector] public bool single { get => _single; }
    [HideInInspector] public bool interacted { get => _interacted; private set => _interacted = value; }
    [HideInInspector] public bool interactionShown { get => _interactionShown; private set => _interactionShown = value; }

    private List<Collider> colliders = new List<Collider>();


    void Awake()
    {
        interacted = false;

        colliders = new List<Collider>(GetComponentsInChildren<Collider>(true));
    }

    // Allows this script to be enabled/disabled in the inspector
    void Update()
    {
        if (CheckMouseOver())
        {
            if (Input.GetMouseButtonDown(0)) Interact();
        }
    }

    void OnDisable() { interactionShown = false; }
    void OnEnable() { interactionShown = true; }
    
    private bool CheckMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (colliders.Contains(hitInfo.collider))
            {
                return true;
            }
        }
        return false;
    }

    public void Interact()
    {
        if (InteractionAllowed())
        {
            Debug.Log("Interact");
            interaction.Invoke();
            interacted = true;
            if (single) interactionShown = false;
        }
        else OnInteractionFailed();
    }
    
    public bool InteractionAllowed()
    {
        return !(single && interacted) && enabled;
    }

    public void OnInteractionFailed()
    {
        interactionFailed.Invoke();
    }
}
