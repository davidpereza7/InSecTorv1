using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;

public class ScenesLoader : MonoBehaviour
{
    public Text nameDisplayText;
    private int pLevel;
    private int quiz1Pass = 0;
    private int level;
    public string tDevice;
    public GameObject LevelPanel;
    public GameObject HomeLevelPanel;
    public GameObject WorkOfficeLevelPanel; 
    public Text playerLevel;
    void OnEnable()
    {
        name = PlayerPrefs.GetString("playername");
                
    }
    void OnDisable()
    {
        
        PlayerPrefs.SetString("playername", name);
        PlayerPrefs.SetInt("playerlevel", level);
        PlayerPrefs.SetString("threatDevice", tDevice);
    }

    public void Start()
    {
        Debug.Log(" El nivel es" +pLevel +"   " + name);
        nameDisplayText.text = name;
        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "SELECT  PLAYER_ID, QUIZ_1_PASS, PLAYER_LEVEL  from PLAYER  where PLAYER_ID ='" + name + "';";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            nameDisplayText.text = (reader.GetString(0));
            quiz1Pass = (reader.GetInt32(1));
            level = (reader.GetInt32(2));
            Debug.Log("el nivel ahora es" + level);
        }

        if (level == 0 && quiz1Pass == 1) { playerLevel.text = "Quiz Level"; }
        if (level == 1 && quiz1Pass == 1) { playerLevel.text = "Home Level"; }
        if (level == 2 && quiz1Pass == 1) { playerLevel.text = "Work Office Level"; }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

     }

    public void starOnLevel()
    {
        if (level == 0 && quiz1Pass == 1) { HomeLevelPanel.gameObject.SetActive(true); }
        if (level == 1 && quiz1Pass == 1) { HomeLevelPanel.gameObject.SetActive(true); }
        if (level == 2 && quiz1Pass == 1) { WorkOfficeLevelPanel.gameObject.SetActive(true); }
       
    }
    
    public void activateLevelPanel()
    {
        
     LevelPanel.gameObject.SetActive(true);

        
    }
    public void LoadSceneRouter()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "ROUTER";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneSmartTV()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "SMART_TV";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneLaptop()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "LAPTOP";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneSocialNetwork()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "SOC_NET";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneDomotic()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "DOMOTICS";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneMobile()
    {
        //Al activar Router Threats se cargaran en la escena los valores que la describen
        tDevice = "MOBILE_PH";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneEmpCom()
    {
        //Al activar Router Threats se cargaran en la escena el titulo sobre email, chats, user profiles, ...
        tDevice = "EMP_COM";
        SceneManager.LoadScene("RouterThreats");
        
    }
    public void LoadSceneSecurity()
    {
        //Al activar Router Threats se cargaran en la escena el titulo sobre proteccion del usuario
        tDevice = "USR_IT_SEC";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneOutConn()
    {
        //Al activar Router Threats se cargaran en la escena el titulo sobre conexiones con el exterior
        tDevice = "OUT_OF_OFF_CONN";
        SceneManager.LoadScene("RouterThreats");
    }
    public void LoadSceneSysServ()
    {
        //Al activar Router Threats se cargaran en la escena el titulo sobre updates, database security, servers,...
        tDevice = "Systems and Servers";
        SceneManager.LoadScene("RouterThreats");
    }
    public void nextLevelInHome()
    {
        SceneManager.UnloadSceneAsync("MinimumReached");
        SceneManager.LoadScene("HomeToDevices");
    }

}
