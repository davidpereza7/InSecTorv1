using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCheckButton : MonoBehaviour
{
    public GameObject CheckButton;
    public void showButton()
    {
        CheckButton.gameObject.SetActive(true);
    }
}

