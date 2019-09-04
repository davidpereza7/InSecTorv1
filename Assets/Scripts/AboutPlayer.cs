using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
public class AboutPlayer : MonoBehaviour
{
    private string playerName;
    private int quiz1Passed;
    private int levelAchieved;
    public GameObject BL1, BL2, BL3, BL4;
    void OnEnable()
    {
       playerName = PlayerPrefs.GetString("playername");
    }

    // Start is called before the first frame update
    void Start()
    {
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select QUIZ_1_PASS, PLAYER_LEVEL from PLAYER WHERE PLAYER_ID = '" + playerName + "'; ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            quiz1Passed = (reader.GetInt32(0));
            levelAchieved = (reader.GetInt32(1));
        }

        Debug.Log("About player" + quiz1Passed + levelAchieved);
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
        //Aqui va el cambio de galones
        Debug.Log(quiz1Passed);

        if (quiz1Passed == 1)
        {
            ColorBlock colorBL1 = BL1.GetComponent<Button>().colors;
            colorBL1.normalColor = new Color32(145, 145, 27, 221);
            BL1.GetComponent<Button>().colors = colorBL1;

            ColorBlock colorBL2 = BL2.GetComponent<Button>().colors;
            colorBL2.normalColor = new Color32(145, 145, 145, 0);
            BL2.GetComponent<Button>().colors = colorBL2;

            ColorBlock colorBL3 = BL3.GetComponent<Button>().colors;
            colorBL3.normalColor = new Color32(145, 145, 145, 0);
            BL3.GetComponent<Button>().colors = colorBL3;

            ColorBlock colorBL4 = BL4.GetComponent<Button>().colors;
            colorBL4.normalColor = new Color32(145, 145, 145, 0);
            BL4.GetComponent<Button>().colors = colorBL4;

        }
        if (quiz1Passed == 1 && levelAchieved == 1)
        {
            ColorBlock colorBL1 = BL1.GetComponent<Button>().colors;
            colorBL1.normalColor = new Color32(145,145,145,0);
            BL1.GetComponent<Button>().colors = colorBL1;

            ColorBlock colorBL2 = BL2.GetComponent<Button>().colors;
            colorBL2.normalColor = new Color32(145, 145, 27, 221);
            BL2.GetComponent<Button>().colors = colorBL2;

            ColorBlock colorBL3 = BL3.GetComponent<Button>().colors;
            colorBL3.normalColor = new Color32(145, 145, 145, 0);
            BL3.GetComponent<Button>().colors = colorBL3;

            ColorBlock colorBL4 = BL4.GetComponent<Button>().colors;
            colorBL4.normalColor = new Color32(145, 145, 145, 0);
            BL4.GetComponent<Button>().colors = colorBL4;
        }
        if (quiz1Passed == 1 && levelAchieved == 2)
        {
            ColorBlock colorBL1 = BL1.GetComponent<Button>().colors;
            colorBL1.normalColor = new Color32(145, 145, 145, 0);
            BL1.GetComponent<Button>().colors = colorBL1;

            ColorBlock colorBL2 = BL2.GetComponent<Button>().colors;
            colorBL2.normalColor = new Color32(145, 145, 145, 0);
            BL2.GetComponent<Button>().colors = colorBL2;

            ColorBlock colorBL3 = BL3.GetComponent<Button>().colors;
            colorBL3.normalColor = new Color32(145, 145, 27, 221);
            BL3.GetComponent<Button>().colors = colorBL3;

            ColorBlock colorBL4 = BL4.GetComponent<Button>().colors;
            colorBL4.normalColor = new Color32(145, 145, 145, 0);
            BL4.GetComponent<Button>().colors = colorBL4;
        }
        if (quiz1Passed == 1 && levelAchieved == 3)
        {
            ColorBlock colorBL1 = BL1.GetComponent<Button>().colors;
            colorBL1.normalColor = new Color32(145, 145, 145, 0);
            BL1.GetComponent<Button>().colors = colorBL1;

            ColorBlock colorBL2 = BL2.GetComponent<Button>().colors;
            colorBL2.normalColor = new Color32(145, 145, 145, 0);
            BL2.GetComponent<Button>().colors = colorBL2;

            ColorBlock colorBL3 = BL3.GetComponent<Button>().colors;
            colorBL3.normalColor = new Color32(145, 145, 145, 0);
            BL3.GetComponent<Button>().colors = colorBL3;

            ColorBlock colorBL4 = BL4.GetComponent<Button>().colors;
            colorBL4.normalColor = new Color32(145, 145, 27, 221);
            BL4.GetComponent<Button>().colors = colorBL4;
        }
    }
}
