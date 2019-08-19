using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsSummary : MonoBehaviour
{
    private float timeRemaining;
    private int questionIndex;
    private int playerScore;
    public Text HomeExitMessage;

    void OnEnable()
    {
        playerScore = PlayerPrefs.GetInt("score");
        questionIndex = PlayerPrefs.GetInt("questions");
        timeRemaining = PlayerPrefs.GetFloat("finaltime");
        name = PlayerPrefs.GetString("playername");
    }


    // Start is called before the first frame update
    void Start()
    {

        HomeExitMessage.text = (name.ToString() + ", you had needed to answer " + questionIndex.ToString() + " questions to achieve 8 correct answers and the remainig time was  " + timeRemaining.ToString("F0") + "  Seconds !!!");
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
