using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideGamePanel: MonoBehaviour
{
    public GameObject GamePanel;
    public void setPanel()
    {
        GamePanel.gameObject.SetActive(true);
    }
}
