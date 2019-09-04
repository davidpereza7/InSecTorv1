using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideReadyButton : MonoBehaviour
{
    public GameObject ReadyButton;
    public void hideButton()
    {
        ReadyButton.gameObject.SetActive(false);
    }
}
