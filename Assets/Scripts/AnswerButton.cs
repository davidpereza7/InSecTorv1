using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Data;
using System.IO;
using UnityEngine.EventSystems;
using Mono.Data.Sqlite;

public class AnswerButton : MonoBehaviour
{

    public Text answerText;
    public int isCorrect;
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
    
        
    
    public void HandleClick()

    {
        //Read the name of the clicked button and then the answer text contained on it

        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("El nombre del boton clickado es " + buttonName);
        string text = GameObject.Find(buttonName).GetComponentInChildren<Text>().text;
        
        //Search  isCorrect condition in the Data Base for the  selected answer is correct
                
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT CORRECT from QUIZ_A  where ANSWER_TXT= '" +text+ "'";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            isCorrect = (reader.GetInt32(0));
        }

        Debug.Log(isCorrect);

        reader.Close();
        reader = null;

        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn = null;
       
        //Returns to the game controller the isCorrect condition once the button is clicked

        gameController.AnswerButtonClicked(isCorrect, buttonName);
        
    }
}
