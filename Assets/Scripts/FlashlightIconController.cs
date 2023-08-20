using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightIconController : MonoBehaviour
{
    [SerializeField] private Image offImage;
    [SerializeField] private Image onImage;

    public void SetState(bool state)
    {
        if (state)
        {
            offImage.gameObject.SetActive(false);
            onImage.gameObject.SetActive(true);
        }
        else
        {
            offImage.gameObject.SetActive(true);
            onImage.gameObject.SetActive(false);
        }
    }
}
