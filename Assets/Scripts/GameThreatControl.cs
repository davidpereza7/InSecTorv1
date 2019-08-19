using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Linq;

public class GameThreadControl : MonoBehaviour
{
    void OnEnable()
    {
        name = PlayerPrefs.GetString("playername");
    }

    public ThreatData threatData = new ThreatData();
    public ThreatCorrectionData threatCorrectionData = new ThreatCorrectionData();
    public GameObject TreatPanel;
    public GameObject ThreatCorrectionPanel;
    public Text threatTextDisplay;
    private int threatCount = 0;
    private int sceneryNumber = 1;
    private int RandThreatNumer = 0;
    
    public void randTreatNumber()
    {
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select COUNT (*) from Threats where SCENERY_ID=" + sceneryNumber + "and Router = 1";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            threatCount = (reader.GetInt32(0));
        }
        //Es asi porque voy a sacar la amenaza que sale del numero aleatorio, la siguiente y la anterior (siempre tendre disponible la primera y la ultima)

        RandThreatNumer = UnityEngine.Random.Range(1, threatCount-1);

        
    }

    
    public String[] threatText = new String[3];
    public void TheThreatText()
    {
        
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT  THREAT_TXT  from THREATS  where THREAT_ID between (" + RandThreatNumer + "- 1) and (" + RandThreatNumer + "+ 1)";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        
        while (reader.Read())
        {
            for (int i = 0; i < 3; i++)
            {
                threatText[i] = (reader.GetString(i));

            }

        }
        threatTextDisplay.text = threatData.ThreatText;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
