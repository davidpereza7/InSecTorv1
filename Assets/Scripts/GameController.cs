﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Linq;


public class GameController : MonoBehaviour
{
    void OnEnable()
    {
        name = PlayerPrefs.GetString("playername");
    }
    void OnDisable()
    {
        PlayerPrefs.SetInt("questions", questionIndex);
        PlayerPrefs.SetInt("score", playerScore);
        PlayerPrefs.SetFloat("finaltime", timeRemaining);
        PlayerPrefs.SetString("playername", name);
    }
    public Text questionDisplayText;
    public Text[] answerText = new Text[3];
    public int[] isCorrect = new int[3];
    public Text scoreDisplayText;
    public Text timeRemainingDisplayText;
    public Text nameDisplayText;
    public QuestionData questionData = new QuestionData();
    public AnswerButton answerButton = new AnswerButton();

    public RoundData roundData = new RoundData();
    //public GameObject questionDisplay;
    private float timeRemaining;
    private int questionIndex;
    private int playerScore;
    

    public GameObject questionDisplay;
    public GameObject roundEndDisplay;
    public GameObject menuScreen;
    // Use this for initialization
    public void Start()
    {
        
        roundData.name = name;
        roundData.timeLimitInSeconds = 200;
        roundData.pointsAddedForCorrectAnswer = 1;
        timeRemaining = roundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();
        nameDisplayText.text =  roundData.name ;
        playerScore = 0;
        questionIndex = 0;
        ShowQuestion();
        
    }

   public void ShowQuestion()
    {
        Randnumber();
        
        GetQ();
        
        GetA();

        
    }
    private int sceneryNumber = 1;
    private int randqNumber = 0;
    private int randqNumberT = 0;
    private int questionCount = 0;
    List<int> RandnumberList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    int i = 0;
    private void Randnumber()
    {
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select COUNT (*) from QUIZ_Q where SCENERY_ID=" + sceneryNumber;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            questionCount = (reader.GetInt32(0));
        }

        Debug.Log(questionCount);

        randqNumberT = UnityEngine.Random.Range(0, questionCount);

        if (!RandnumberList.Contains(randqNumberT))
        {
            RandnumberList.Add(randqNumberT);
            randqNumber = randqNumberT;

        }
        else
        {
            Randnumber();
        }

                
        reader.Close();
        reader = null;

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;

    }

    private void GetQ() //Saca preguntas de la base de datos

    {


        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT  Q_TXT  from QUIZ_Q  where Q_ID =" + randqNumber;

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            questionData.questionText = (reader.GetString(0));
        }
        questionDisplayText.text = questionData.questionText;

        Debug.Log("La pregunta es" + questionData.questionText);


        reader.Close();
        reader = null;

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;

    }

    private void GetA() //Posibles respuestas (3) para la pregunta
    {
        //Genera un numero aleatorio pra que las respuestas a la misma pregunta no aparezcan en la misma posición
        //en el answerdisplay
        List<int> Lista1 = new List<int> { 0, 1, 2 };
        List<int> Lista2 = new List<int> { 1, 2, 0 };
        List<int> Lista3 = new List<int> { 2, 1, 0 };
        List<int> Lista4 = new List<int> { 0, 2, 1 };
        List<int> Lista5 = new List<int> { 1, 0, 2 };
        List<int> Lista6 = new List<int> { 2, 0, 1 };
        int r = UnityEngine.Random.Range(1, 6);
        Debug.Log("El numero aleatorio es " + r);
        int[] Listin = new int[6];
        if (r == 1)
        {
            Lista1.CopyTo(Listin);
        }
        if (r == 2)
        {
            Lista2.CopyTo(Listin);
        }
        if (r == 3)
        {
            Lista3.CopyTo(Listin);
        }
        if (r == 4)
        {
            Lista4.CopyTo(Listin);
        }
        if (r == 5)
        {
            Lista5.CopyTo(Listin);
        }
        if (r == 6)
        {
            Lista6.CopyTo(Listin);
        }
               
        Debug.Log("La lista utilizada es "+ Listin);
        Debug.Log("Los numeros aleatorios son" + Listin[0] +Listin[1]+Listin[2]);


        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();

        
       

        for (int i = 0; i < 3; i++)
        {
                int q = (Listin[i]);
                Debug.Log("El valor de q es "+q);
                //Debug.Log("Las variables para buscar las respuesta y la de colocacion son" + i + q);
                IDbCommand dbcmd = dbconn.CreateCommand();
                string sqlQuery = "SELECT  ANSWER_TXT, CORRECT from QUIZ_A  where Q_ID =" + randqNumber + " AND ANSWER_ID_ID = " + i;

                dbcmd.CommandText = sqlQuery;
                IDataReader reader = dbcmd.ExecuteReader();

                while (reader.Read())
                {
                    Debug.Log("Las variables para buscar las respuesta y la de colocacion son" + i + q);
                    questionData.answers[i] = new AnswerData { answerText = (reader.GetString(0)), isCorrect = (reader.GetInt32(1)) };
                    Debug.Log("Las repuestas son las siguientes" + questionData.answers[i].answerText);
                    answerText[q].text = questionData.answers[i].answerText;
                }
           

            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;


        }
        dbconn.Close();
        dbconn = null;

    }
    
       
    public void AnswerButtonClicked(int isCorrect, string buttonName)
    {
        Debug.Log("Los indicadores para decidir son   " + isCorrect +  playerScore   + questionIndex);

        if (questionIndex > 9) { EndRound();}
        if (playerScore == 7) {SceneManager.LoadScene("MinimumReached");}
        if (isCorrect == 1)
                {
                    playerScore += roundData.pointsAddedForCorrectAnswer;
                    scoreDisplayText.text = playerScore.ToString();
                    questionIndex++;
                    ShowQuestion();
                    }
                else
                    {
                    questionIndex++;
                    ShowQuestion();
                    }      
        }

    public void EndRound()
    {
        questionDisplay.SetActive(false);
        roundEndDisplay.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = Mathf.Round(timeRemaining).ToString();
    }

     
    void Update()
    {
        
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if (timeRemaining <= 0f)
            {
                EndRound();
            }
        
    }
}
    
