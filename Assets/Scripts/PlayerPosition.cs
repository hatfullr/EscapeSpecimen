using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public ViewingAngle firstView;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private bool isFirst = false;



    [Header("Debugging")]
    [SerializeField, ReadOnly] private ViewingAngle _currentViewingAngle;
    [SerializeField, ReadOnly] private int viewIndex;
    [ReadOnly] public PlayerController player;


    [HideInInspector] public ViewingAngle currentViewingAngle { get => _currentViewingAngle; private set => _currentViewingAngle = value; } 

    private List<ViewingAngle> _viewingAngles;
    private List<ViewingAngle> viewingAngles
    {
        get
        {
            if (_viewingAngles == null) _viewingAngles = new List<ViewingAngle>(GetComponentsInChildren<ViewingAngle>(true));
            return _viewingAngles;
        }
    }

    void Start()
    {
        if (isFirst)
        {
            GameObject go = null;
            go = Instantiate(playerObject, null);
            PlayerController controller = go.GetComponent<PlayerController>();
            MovePlayerToThisPosition(controller);
            ChangeView(firstView);
        }
    }

    public void TurnLeft()
    {
        if (viewIndex <= 0)
        {
            viewIndex = viewingAngles.Count - 1;
        }
        else viewIndex--;
        ChangeView(viewingAngles[viewIndex]);
    }

    public void TurnRight()
    {
        if (viewIndex >= viewingAngles.Count - 1) // Wrap around
        {
            viewIndex = 0;
        }
        else viewIndex++;
        ChangeView(viewingAngles[viewIndex]);
    }


    public void MovePlayerToThisPosition(PlayerController controller)
    {
        controller.position = this;
        
    }

    public void ChangeView(ViewingAngle view)
    {
        if (view == null) throw new System.Exception("Given a null view");
        if (!viewingAngles.Contains(view)) throw new System.Exception("Given view is not in the list of possible viewing angles for this position");
        player.transform.rotation = view.transform.rotation;
        currentViewingAngle = view;
        viewIndex = viewingAngles.IndexOf(currentViewingAngle);
    }
}
