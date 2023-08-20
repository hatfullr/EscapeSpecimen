using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerPosition : MonoBehaviour
{
    public ViewingAngle firstView;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private bool isFirst = false;
    [SerializeField] private bool wrapViewingAngles = false;
    [SerializeField] private GameObject indicator;

    public UnityEvent onPlayerEnter;
    public UnityEvent onPlayerExit;

    [Header("Debugging")]
    [SerializeField, ReadOnly] private ViewingAngle _currentViewingAngle;
    [SerializeField, ReadOnly] private int viewIndex;
    [ReadOnly] public PlayerController player;


    [HideInInspector] public ViewingAngle currentViewingAngle { get => _currentViewingAngle; private set => _currentViewingAngle = value; } 

    [HideInInspector] public List<ViewingAngle> viewingAngles;

    void Awake()
    {
        viewingAngles = new List<ViewingAngle>(GetComponentsInChildren<ViewingAngle>(true));
        if (firstView != null) currentViewingAngle = firstView;
        else currentViewingAngle = viewingAngles[0];
    }

    void Start()
    {
        if (isFirst)
        {
            GameObject go = null;
            go = Instantiate(playerObject, null);
            PlayerController controller = go.GetComponent<PlayerController>();
            controller.SetPosition(this, true);
        }
    }

    public void TurnRight(PlayerController controller)
    {
        // Disallow wrapping around
        if (viewIndex <= 0)
        {
            if (!wrapViewingAngles) return;
            viewIndex = viewingAngles.Count;
        }
        viewIndex--;
        ChangeView(controller, viewingAngles[viewIndex]);
    }

    public void TurnLeft(PlayerController controller)
    {
        // Disallow wrapping around
        if (viewIndex >= viewingAngles.Count - 1)
        {
            if (!wrapViewingAngles) return;
            viewIndex = -1;
        }
        viewIndex++;
        ChangeView(controller, viewingAngles[viewIndex]);
    }

    public void ChangeView(PlayerController controller, ViewingAngle view)
    {
        if (view == null) throw new System.Exception("Given a null view");
        if (!viewingAngles.Contains(view)) throw new System.Exception("Given view is not in the list of possible viewing angles for this position");
        controller.transform.rotation = view.transform.rotation;
        currentViewingAngle = view;
        viewIndex = viewingAngles.IndexOf(currentViewingAngle);
    }


    public void SetIndicator(bool state)
    {
        if (indicator) indicator.SetActive(state);
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (isFirst)
            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 100f);
    }
#endif

}
