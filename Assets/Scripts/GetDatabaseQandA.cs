using System;
using System.Data;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;




public class GetDatabaseQandA : MonoBehaviour
{
    private int sceneryNumber = 1;
    private int randqNumber = 0;
    private int questionCount = 0;

    public QuestionData questionData = new QuestionData();

    public void Randnumber()
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

        randqNumber = UnityEngine.Random.Range(0, questionCount);

        Debug.Log("el numero aleatorio es " + randqNumber);

        reader.Close();
        reader = null;

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;

    }

    public void GetQ() //Saca preguntas de la base de datos

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

        Debug.Log("La pregunta es" + questionData.questionText);


        reader.Close();
        reader = null;

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;

    }
    public void GetA() //Posibles respuestas (3) para la pregunta
    {

        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        


        for (int i = 0; i < 3; i++)

        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT  ANSWER_TXT, CORRECT from QUIZ_A  where Q_ID =" + randqNumber + " AND ANSWER_ID_ID = " + i;
           
            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();
           
            while (reader.Read())
                    {
                        questionData.answers[i] = new AnswerData { answerText = (reader.GetString(0)), isCorrect = (reader.GetInt32(1)) };

                        Debug.Log("El contenido del array es... " + questionData.answers[i].answerText.ToString() + questionData.answers[i].isCorrect.ToString());
            }


            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;

            
        }
        dbconn.Close();
        dbconn = null;

        
    }
}
