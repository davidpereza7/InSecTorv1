using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideAnswers : MonoBehaviour
{
    public GameObject Answers;
    public void setPanel()
    {
        Answers.gameObject.SetActive(true);
    }
}
