using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Reference the Fade Image under the Player HUD in the object/prefab Heirarchy
    public Image fadeImage;

    [Header("Debugging")]
    [ReadOnly] public PlayerPosition position;




    void Update()
    {
        //only do these if the player is not going through a fade transition
        if (Input.GetKeyDown(KeyCode.W)) MoveForward();
        if (Input.GetKeyDown(KeyCode.S)) MoveBackwards();
        if (Input.GetKeyDown(KeyCode.A)) TurnLeft();
        if (Input.GetKeyDown(KeyCode.D)) TurnRight();
    }

    private void MoveForward()
    {
        Debug.Log("Move Forward", gameObject);
        SetPosition(position.currentViewingAngle.moveForward);
        StartCoroutine("MoveTransition");
    }

    private void MoveBackwards()
    {
        Debug.Log("Move Backwards", gameObject);
    }

    private void TurnLeft()
    {
        Debug.Log("Turn Left", gameObject);
        position.TurnLeft(this);
    }

    private void TurnRight()
    {
        Debug.Log("Turn Right", gameObject);
        position.TurnRight(this);
    }



    public void SetPosition(PlayerPosition position)
    {
        this.position = position;
        position.player = this;

        transform.position = position.transform.position;

        // Change this later when we want to preserve player direction on move
        SetView(position.firstView);
    }

    public void SetView(ViewingAngle view)
    {
        view.playerPosition.ChangeView(this, view);
    }

    /// <summary>
    /// Lerps transparency on Fade Image over t(time) to create the fade in/out when moving
    /// </summary>
    private IEnumerator MoveTransition()
    {
        //Toggle player input off

        float fadeInOutTime = 3;
        float waitTime = 1;

        //quick find since I can't seem to get the reference box to show in the script inspector
        if (fadeImage == null)
        {
            fadeImage = GameObject.Find("Fade Image").GetComponent<Image>();
        }
        Color fadeColor = fadeImage.color;
        float fadeAmount;

        //Fade to black
        while (fadeImage.color.a <= 1)
        {
            fadeAmount = fadeColor.a + (fadeInOutTime * Time.deltaTime);
            fadeColor.a = fadeAmount;
            fadeImage.color = fadeColor;
            yield return null;
        }
        //Move Player
        //Wait a moment
        while(waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
            yield return null;
        }
        //Fade in
        while (fadeImage.color.a >= 0)
        {
            fadeAmount = fadeColor.a - (fadeInOutTime * Time.deltaTime);
            fadeColor.a = fadeAmount;
            fadeImage.color = fadeColor;
            yield return null;
        }
        //Toggle player input on.
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = Camera.main.transform.position;
        Gizmos.DrawLine(origin, origin + transform.forward * 20f);
    }
#endif
}
