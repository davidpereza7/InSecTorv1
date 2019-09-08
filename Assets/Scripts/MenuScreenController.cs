using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class MenuScreenController : MonoBehaviour
{
    public RoundData roundData = new RoundData();
    public Text playerName;
    public GameObject StartInsecTor;
    public GameObject RegisterButton;
    public GameObject ScrollText;
    

    public string tempPlayerName;
    public string returnedPlayerName;
    public int quiz1Pass = 0;
    private int playerLevel;
    public Text wellcomeText;
    private string playerLevelText;

    void OnDisable()
    {
        PlayerPrefs.SetString("playername", playerName.text);
        PlayerPrefs.SetInt("playerLevel", playerLevel);
    }
    public void registerPlayerName()
    {
        tempPlayerName = playerName.text.ToString();
        roundData.name = playerName.text.ToString();

        Debug.Log("el nombre del jugador es " + tempPlayerName);

        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select PLAYER_ID, QUIZ_1_PASS, PLAYER_LEVEL from PLAYER WHERE PLAYER_ID = '" + tempPlayerName + "'; ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            returnedPlayerName = (reader.GetString(0));
            quiz1Pass = (reader.GetInt32(1));
            playerLevel = (reader.GetInt32(2));
        }
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

        if (returnedPlayerName == tempPlayerName)
        {
            welcomeMessage();
            
        }
        else
        {
            string conn1 = "URI=file:" + Application.dataPath + "/plugins/insector.db";
            
            IDbConnection dbconn1;
            dbconn1 = (IDbConnection)new SqliteConnection(conn);
            dbconn1.Open();
            IDbCommand dbcmd1 = dbconn1.CreateCommand();

            string sqlQuery1 = "INSERT INTO PLAYER (PLAYER_ID, QUIZ_1_PASS, THREATS_SCORE, CORRECTIONS_SCORE, PLAYER_LEVEL) VALUES ('" + tempPlayerName + "', 0, 0, 0, 0); ";
            Debug.Log(sqlQuery1);
            dbcmd1.CommandText = sqlQuery1;
            IDataReader reader1 = dbcmd1.ExecuteReader();
            reader1.Close();
            reader1 = null;
            dbcmd1.Dispose();
            dbcmd1 = null;
            dbconn1.Close();
            dbconn1 = null;

            playerName.text = tempPlayerName;
            registeredMessage();

        }
    }

    public void AgainHome()
    {
        
        SceneManager.LoadScene("Home");
    }

    public void Leave()
    {
        SceneManager.LoadScene("MenuScreen");
    }
    public void StartGame()
    {
        if (quiz1Pass == 1)
        {
            SceneManager.LoadScene("HomeToDevices");

        }
        else
        {
            SceneManager.LoadScene("Home");
        }
      
    }

    public void welcomeMessage()
    {
        wellcomeText.text = "Welcome again,  " + returnedPlayerName;
        roundData.name = returnedPlayerName;
    }
    public void registeredMessage()
    {
        wellcomeText.text =  tempPlayerName + " , registered";
        roundData.name = returnedPlayerName;
    }
    public void setStartInsecTor()
    {
        StartInsecTor.gameObject.SetActive(true);
        RegisterButton.gameObject.SetActive(false);
    }
    public void setHowToPlay()
    {
        if (ScrollText.activeSelf is true)
        {
            ScrollText.gameObject.SetActive(false);
        }
        else
        {
            ScrollText.gameObject.SetActive(true);
        }
        
    }
}