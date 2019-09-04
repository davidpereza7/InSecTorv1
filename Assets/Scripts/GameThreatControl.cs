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

public class GameThreatControl : MonoBehaviour
{

    /////////////////////////////////////////////////////////
    //
    //  Variable definitions and references to game objects
    //
    /////////////////////////////////////////////////////////

    public TCorrectionDataA tCorrectionDataA = new TCorrectionDataA();
    public ThreatDataA threatDataA = new ThreatDataA();
    public GameObject threatPanel;
    public GameObject deviceName;
    public GameObject timePanel;
    public GameObject ThreatCorrectionsPanel;
    public GameObject ThreatErrorPanel;
    public GameObject LevelPanel;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject GameInstructionsPanel;
    private int threatCount = 0;
    private int sceneryNumber = 1;
    private int RandThreatNumer = 0;
    public Text[] tDisplayText = new Text[3];
    public Text[] tCorrectionDisplayText = new Text[3];
    public int threatsScore;
    public int correctionsScore;
    public int playerLevel;
    private int quiz_1_Pass;
    private int selectedThreatId;
    public Text nameDisplayText;
    public Text tScoreDisplayText;
    public Text cScoreDisplayText;
    public Text deviceDisplayText;
    private string nameDevice;
    private string playerName;
    private Text mDevice;
    Boolean proper1, proper2, proper3, proper4;
    Boolean correct1, correct2, correct3, correct4;
    Boolean properc1, properc2, properc3, properc4;
    Boolean correctc1, correctc2, correctc3, correctc4;
    public GameObject Threat1, Threat2, Threat3, Threat4;
    public GameObject ThreatCorrection1, ThreatCorrection2, ThreatCorrection3, ThreatCorrection4;
    private int properCount = 0;
    private int propercCount = 0;
    private int randProper;
    private string selectedThreat;
    private int randCorrectionNumber = 2;
    private string validForThreats;

    /////////////////////////////////////////////////////////
    //
    //  Getting and seting variables from/to other scenes or scripts
    //
    /////////////////////////////////////////////////////////

    void OnDisable()
    {
        //PlayerPrefs.SetInt("playerlevel", playerLevel);
    }

    void OnEnable()
    {
        playerName = PlayerPrefs.GetString("playername");
        nameDevice = PlayerPrefs.GetString("threatDevice");
    }

    /////////////////////////////////////////////////////////
    //
    //  Actions taking place once the panel is active
    //
    /////////////////////////////////////////////////////////
    void Start()
    {
        
        hideCorrectionsPanel();
        //Se inicializan las variables de estado, dependiendo del dispositivo por el que hayamos entrado
        //tDevice, threatGameLevel, playerName, score si hubiese historia del jugador (Incluido en la base de datos del perfile del jugador)
        //Es asi porque voy a sacar la amenaza que sale del numero aleatorio, la siguiente y la anterior (siempre tendre disponible la primera y la ultima)

        SetGameInstructionsPanel();

        //Se busca en la base de datos la descripción del dispositivo para sacarlo en la pantalla

        string conn3 = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn3;
        dbconn3 = (IDbConnection)new SqliteConnection(conn3);
        dbconn3.Open();
        IDbCommand dbcmd3 = dbconn3.CreateCommand();

        string sqlQuery3 = "select DEVICE_DESCRIP from DEVICES where DEVICE_NAME ='" + nameDevice +"';";
        dbcmd3.CommandText = sqlQuery3;
        IDataReader reader3 = dbcmd3.ExecuteReader();

        while (reader3.Read())
        {
            deviceDisplayText.text = (reader3.GetString(0));
        }
        
        reader3.Close();
        reader3 = null;
        dbcmd3.Dispose();
        dbcmd3 = null;
        dbconn3.Close();
        dbconn3 = null;

               
        // Get player history from Database

        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select QUIZ_1_PASS, THREATS_SCORE, CORRECTIONS_SCORE, PLAYER_LEVEL from PLAYER WHERE PLAYER_ID = '" + playerName + "'; ";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            quiz_1_Pass = (reader.GetInt32(0));
            threatsScore = (reader.GetInt32(1));
            correctionsScore = (reader.GetInt32(2));
            playerLevel = (reader.GetInt32(3));
        }

        //Set the player initial conditions

        nameDisplayText.text = playerName;
        tScoreDisplayText.text = threatsScore.ToString();
        cScoreDisplayText.text = correctionsScore.ToString();
        Debug.Log(playerName + threatsScore + correctionsScore);

        if (playerLevel == 1)
        {
            Level1.GetComponent<Text>().color = Color.red;
            Level2.GetComponent<Text>().color = Color.gray;
        }

        if (playerLevel == 2)
        {
            Level1.GetComponent<Text>().color = Color.gray;
            Level2.GetComponent<Text>().color = Color.red;
        }
    }


    /////////////////////////////////////////////////////////
    //                  SHOW THREATS
    //                  THREATS PANEL
    //  Get the pannel ready and show the list of threats
    //
    /////////////////////////////////////////////////////////
    public void ShowThreats()
    {
        Threat1.GetComponentInChildren<Toggle>().isOn = false;
        Threat2.GetComponentInChildren<Toggle>().isOn = false;
        Threat3.GetComponentInChildren<Toggle>().isOn = false;
        Threat4.GetComponentInChildren<Toggle>().isOn = false;
        

        randThreatNumber();
        TheThreatText();
               
    }


    /////////////////////////////////////////////////////////
    //                  SHOW CORRECTIONS
    //                  CORRECTIONS PANEL
    //  Get the corrections pannel ready and show the list of Corrections
    //
    /////////////////////////////////////////////////////////
    public void ShowCorrections()
    {
        hideCorrectionsPanel();
        setCorrectionsPanel();
        TheCorrectionsText();
    }

    /////////////////////////////////////////////////////////
    //                  PROPER THREATS
    //              THREAT PANEL. PROPER SELECTION
    //  Check if the player selection is the appropiate for the device
    //
    /////////////////////////////////////////////////////////

    public void properThreats()
    {
        //Se recogen los Toggles activados y se comprueba si se han activado correctamente y se cuentan
        
        if (threatDataA.threats[0].ofThreat == 1 && Threat1.GetComponent<Toggle>().isOn == true) { proper1 = true; correct1 = true; }
        if (threatDataA.threats[0].ofThreat == 0 && Threat1.GetComponent<Toggle>().isOn == false) { proper1 = false; correct1 = true; }
        if (threatDataA.threats[1].ofThreat == 1 && Threat2.GetComponent<Toggle>().isOn == true) { proper2 = true; correct2 = true; }
        if (threatDataA.threats[1].ofThreat == 0 && Threat2.GetComponent<Toggle>().isOn == false) { proper2 = false ;correct2 = true; }
        if (threatDataA.threats[2].ofThreat == 1 && Threat3.GetComponent<Toggle>().isOn == true) { proper3 = true; correct3 = true; }
        if (threatDataA.threats[2].ofThreat == 0 && Threat3.GetComponent<Toggle>().isOn == false) { proper3 = false; correct3 = true; }
        if (Threat4.GetComponent<Toggle>().isOn == true &&
                (threatDataA.threats[0].ofThreat == 0) && 
                (threatDataA.threats[1].ofThreat == 0) &&
                (threatDataA.threats[2].ofThreat == 0))
                    {
                        proper4 = true;
                        correct4 = true;
                    }
     
        if (Threat4.GetComponent<Toggle>().isOn == false &&
                (threatDataA.threats[0].ofThreat == 1) ||
                (threatDataA.threats[1].ofThreat == 1) ||
                (threatDataA.threats[2].ofThreat == 1))
                    {
                        proper4 = false;
                        correct4 = true;
                    }
        
        
        if (correct4 == true && proper1 == false && proper2 == false && proper3 == false && proper4 == true)
        {
            properCount = properCount + 1;
            threatsScore = threatsScore + properCount;
            Debug.Log(propercCount);
            tScoreDisplayText.text = properCount.ToString();
            ShowThreats();
            //Si acierta todo los proper threats de la lista (Marcando los toggle correctos) seapunta 3 y se va a showCorrections
        }

        if ((correct1 == true) && (correct2 == true) && (correct3 == true) && (correct4 == true))
        {
            if (proper1 == true) { properCount = properCount + 1; }
            if (proper2 == true) { properCount = properCount + 1; }
            if (proper3 == true) { properCount = properCount + 1; }
            randProper = UnityEngine.Random.Range(1, properCount);
            //Se selecciona la amenaza para la que se van a presentar correciones
            selectedThreat = threatDataA.threats[randProper].threatText;

            threatsScore = threatsScore + properCount;
            tScoreDisplayText.text = threatsScore.ToString();

            ShowCorrections();
        }


        else
            {
            Debug.Log("he pasado");
            SetThreatErrorPanel();
            }
    
    }

    /////////////////////////////////////////////////////////
    //                RAND THREAT NUMBER
    //                  THREATS  PANEL
    //  Random number to select the set of threats to show in the panel
    //
    /////////////////////////////////////////////////////////
    private void randThreatNumber()

        {

        if (playerLevel == 1) { sceneryNumber = 1; }
        if (playerLevel == 2) { sceneryNumber = 2; }

        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();

        string sqlQuery = "select COUNT (*) from Threats where SCENERY_ID=" + sceneryNumber ;
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            threatCount = (reader.GetInt32(0));
        }

        //Es asi porque voy a sacar la amenaza que sale del numero aleatorio, la siguiente y la anterior (siempre tendre disponible la primera y la ultima)

        if (sceneryNumber == 1) { RandThreatNumer = UnityEngine.Random.Range(2, threatCount - 1); }
        if (sceneryNumber == 2) { RandThreatNumer = UnityEngine.Random.Range(2, threatCount - 1);
            RandThreatNumer = RandThreatNumer + 100;
                                }
        Debug.Log(threatCount);
        Debug.Log(RandThreatNumer);
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;

    }

    /////////////////////////////////////////////////////////
    //               THE THREAT TEXT
    //                THREATS  PANEL
    //  Prepare an array with the threats selected to show
    //
    /////////////////////////////////////////////////////////
    public void TheThreatText()

    {
        
        string conn1 = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn1;
        dbconn1 = (IDbConnection)new SqliteConnection(conn1);
        dbconn1.Open();
        int i, q;
        for (i = (RandThreatNumer - 1), q = 0; i < (RandThreatNumer + 2) && q < 3; i++, q++)
        {
            
                IDbCommand dbcmd1 = dbconn1.CreateCommand();
            string sqlQuery1 = "SELECT  THREAT_TXT ," + nameDevice + " from THREATS  where THREAT_ID = " + i;
                //string sqlQuery1 = "SELECT  THREAT_TXT from THREATS  where THREAT_ID = " + i + " and SCENERY_ID =" + sceneryNumber;
            dbcmd1.CommandText = sqlQuery1;
                IDataReader reader1 = dbcmd1.ExecuteReader();

                while (reader1.Read())
                {
                    threatDataA.threats[q] = new ThreatData { threatText = (reader1.GetString(0)), ofThreat = (reader1.GetInt32(1)) };
                    tDisplayText[q].text = threatDataA.threats[q].threatText;
                }
            Threat4.GetComponentInChildren<Text>().text = "Nothing to do with this device";
            reader1.Close();
            reader1 = null;

            dbcmd1.Dispose();
            dbcmd1 = null;
        }
        dbconn1.Close();
        dbconn1 = null;

    }


    /////////////////////////////////////////////////////////
    //            THE CORRECTIONS TEXT
    //              CORRECTIONS PANEL
    //  Selection of different options of corections to the proposed threat
    //  and get ready the threats and corrections panels
    //
    /////////////////////////////////////////////////////////

    public void TheCorrectionsText()
    //Se sacan da la base de datos Tabla THREAT_SOLUTION las correcciones de las amenazas
    //Para probar sacamos tres amenazas a lo bruto sin tener en cuenta la amenaza ni el dispositvo ni el nivel del juego
    //Primero pongo en blando las etiquetas y los toggles que o son la amenaza en cuestion
    //Selecciono de la base de datos tres correciones y compruebo si son correctas para la amenaza
    //Cuento las correctas o un punto si ninguna
    //Vuelvo a llamar a show threats
    {
        //se obtiene el id de la selected threat

        string conn1 = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn1;
        dbconn1 = (IDbConnection)new SqliteConnection(conn1);
        dbconn1.Open();
        IDbCommand dbcmd1 = dbconn1.CreateCommand();

        string sqlQuery1 = "SELECT THREAT_ID  from THREATS where THREAT_TXT = '"+selectedThreat+ "';";
        dbcmd1.CommandText = sqlQuery1;
        IDataReader reader1 = dbcmd1.ExecuteReader();
                
        while (reader1.Read())
        {
            selectedThreatId = (reader1.GetInt32(0));
        }
               
        //Es asi porque voy a sacar la amenaza que sale del numero aleatorio, la siguiente y la anterior (siempre tendre disponible la primera y la ultima)

        RandThreatNumer = UnityEngine.Random.Range(2, threatCount - 1);
        Debug.Log("el valor de RandThreatNumber es...." + RandThreatNumer);
        reader1.Close();
        reader1 = null;
        dbcmd1.Dispose();
        dbcmd1 = null;
        dbconn1.Close();
        dbconn1 = null;

        //Con el valor de la selectedThreat ya se pueden hacer las busquedas de las correcciones aleatoriamente 
                     
        hideGameInstructionsPanel();

        Threat4.GetComponentInChildren<Text>().text = "";
        Threat4.GetComponentInChildren<Toggle>().isOn = false;

        if (Threat1.GetComponentInChildren<Text>().text != selectedThreat)
        {
            Threat1.GetComponentInChildren<Text>().text = "";
            Threat1.GetComponentInChildren<Toggle>().isOn = false;
        }
        if (Threat2.GetComponentInChildren<Text>().text != selectedThreat)
        {
            Threat2.GetComponentInChildren<Toggle>().isOn = false;
            Threat2.GetComponentInChildren<Text>().text = "";
        }
        if (Threat3.GetComponentInChildren<Text>().text != selectedThreat)
        {
            Threat3.GetComponentInChildren<Toggle>().isOn = false;
            Threat3.GetComponentInChildren<Text>().text = "";
        }

        string conn = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        int t;

        for (t = 0; t < 3;  t++)
        {
            IDbCommand dbcmd = dbconn.CreateCommand();
            //select de tres correciones. El numero de correctas será aleatorio. Se hara con dos select
            //una para las aleatorias correctas y otra para las aleatorias incorrectas ( A las correctas
            //les pondremos isCorrect a 1. La posicion con las que se presentarán en la pantalla será tambien aleatoria

            //string sqlQuery = "SELECT THREAT_SOLUTION_TXT, THREAT_CORRECTION_ID, VALID_FOR_THREAT FROM THREAT_SOLUTION where ROUTER = 1 and THREAT_CORRECTION_LEVEL = 1 AND instr ( VALID_FOR_THREAT, '1-')  ORDER BY random() LIMIT 1;";
            //Primero se prueba si seleccionar la correccion

            string sqlQuery = "SELECT THREAT_SOLUTION_TXT, THREAT_CORRECTION_ID, VALID_FOR_THREAT FROM THREAT_SOLUTION where " +nameDevice +"= 1 and THREAT_CORRECTION_LEVEL = 1  ORDER BY random() LIMIT 1;";

            dbcmd.CommandText = sqlQuery;
            IDataReader reader = dbcmd.ExecuteReader();

            while (reader.Read())
            {
                validForThreats = (reader.GetString(2));
                
                if (validForThreats.Contains(selectedThreatId + "-"))
                {
                    tCorrectionDataA.corrections[t] = new TCorrectionData { threatCorrectionText = (reader.GetString(0)), isCorrect = 1 };
                }
                else
                {
                    tCorrectionDataA.corrections[t] = new TCorrectionData { threatCorrectionText = (reader.GetString(0)), isCorrect = 0 };
                }
                
                tCorrectionDisplayText[t].text = tCorrectionDataA.corrections[t].threatCorrectionText;
        
            }
            reader.Close();
            reader = null;

            dbcmd.Dispose();
            dbcmd = null;
        }
        dbconn.Close();
        dbconn = null;
        
    }

    /////////////////////////////////////////////////////////
    //          PROPER CORRECTIONS       
    //      CORRECTIONS PANEL. CHECK SELECTION
    //  Check if the player answers are the correct ones for the threat selected
    //
    /////////////////////////////////////////////////////////
    public void properCorrections()
    {

        //Se recogen los Toggles activados y se comprueba si se han activado correctamente y se cuentan

        if (tCorrectionDataA.corrections[0].isCorrect == 1 && ThreatCorrection1.GetComponent<Toggle>().isOn == true) { properc1 = true; correctc1 = true; }
        if (tCorrectionDataA.corrections[0].isCorrect == 0 && ThreatCorrection1.GetComponent<Toggle>().isOn == false) { properc1 = false; correctc1 = true; }
        if (tCorrectionDataA.corrections[1].isCorrect == 1 && ThreatCorrection2.GetComponent<Toggle>().isOn == true) { properc2 = true; correctc2 = true; }
        if (tCorrectionDataA.corrections[1].isCorrect == 0 && ThreatCorrection2.GetComponent<Toggle>().isOn == false) { properc2 = false; correctc2 = true; }
        if (tCorrectionDataA.corrections[2].isCorrect == 1 && ThreatCorrection3.GetComponent<Toggle>().isOn == true) { properc3 = true; correctc3 = true; }
        if (tCorrectionDataA.corrections[2].isCorrect == 0 && ThreatCorrection3.GetComponent<Toggle>().isOn == false) { properc3 = false; correctc3 = true; }
        if (Threat4.GetComponent<Toggle>().isOn == true &&
                (tCorrectionDataA.corrections[0].isCorrect == 0) &&
                (tCorrectionDataA.corrections[1].isCorrect == 0) &&
                (tCorrectionDataA.corrections[2].isCorrect == 0))
        {
            properc4 = true;
            correctc4 = true;
        }

        if (ThreatCorrection4.GetComponent<Toggle>().isOn == false &&
                (tCorrectionDataA.corrections[0].isCorrect == 1) ||
                (tCorrectionDataA.corrections[1].isCorrect == 1) ||
                (tCorrectionDataA.corrections[2].isCorrect == 1))
        {
            properc4 = false;
            correctc4 = true;

            
        }
        
        
        //Si acierta todo los proper threats de la lista (Marcando los toggle correctos) seapunta 3 y se va a showCorrections
        if (correctc1 == true && correctc2 == true && correctc3 == true && correctc4 == false && properc1 == false && properc2 == false && properc3 == false && properc4 == false)
        {
            propercCount = propercCount + 1;
            correctionsScore = correctionsScore + propercCount;
            cScoreDisplayText.text = correctionsScore.ToString();
            hideCorrectionsPanel();
            ShowThreats();
        }

        if (correctc4 == true && properc1 == false && properc2 == false && properc3 == false)
        {
            propercCount = propercCount + 1;
            correctionsScore = correctionsScore + propercCount;
            cScoreDisplayText.text = correctionsScore.ToString();
            cleanCorrectionsToggles();
            hideCorrectionsPanel();
            ShowThreats();
        }
        if ((correctc1 == true) && (correctc2 == true) && (correctc3 == true) && (correctc4 == true))
        {
            if (properc1 == true) { propercCount = propercCount + 1; }
            if (properc2 == true) { propercCount = propercCount + 1; }
            if (properc3 == true) { propercCount = propercCount + 1; }
            correctionsScore = correctionsScore + propercCount;
            cScoreDisplayText.text = correctionsScore.ToString();
            cleanCorrectionsToggles();
            hideCorrectionsPanel();
            ShowThreats();
        }
                
            cleanCorrectionsToggles();
            hideCorrectionsPanel();
            SetThreatErrorPanel();
       
    }

    /////////////////////////////////////////////////////////
    //             SAVE AND EXIT BUTTON
    //  Save the score in the player table
    //
    /////////////////////////////////////////////////////////
    public void saveAndExit()
    {
        //Se guarda el score del jugador y se ponen los contadores a cero
        //Se presenta de nuevo la pantalla "start"
        if (threatsScore == 40 && correctionsScore == 30)
        {
            playerLevel = 2;
        }
        string conn1 = "URI=file:" + Application.dataPath + "/plugins/insector.db";
        IDbConnection dbconn1;
        dbconn1 = (IDbConnection)new SqliteConnection(conn1);
        dbconn1.Open();
        IDbCommand dbcmd1 = dbconn1.CreateCommand();

        string sqlQuery1 = "UPDATE PLAYER SET THREATS_SCORE =" + threatsScore + ", CORRECTIONS_SCORE =" + correctionsScore + ", PLAYER_LEVEL =" + playerLevel + " WHERE PLAYER_ID ='" + playerName + "';";
        Debug.Log(sqlQuery1);
        dbcmd1.CommandText = sqlQuery1;
        IDataReader reader1 = dbcmd1.ExecuteReader();


        reader1.Close();
        reader1 = null;
        dbcmd1.Dispose();
        dbcmd1 = null;
        dbconn1.Close();
        dbconn1 = null;

        SceneManager.LoadScene("MenuScreen");
    }
    public void setCorrectionsPanel()
    {
        ThreatCorrectionsPanel.gameObject.SetActive(true);

    }
    public void hideCorrectionsPanel()
    {
        ThreatCorrectionsPanel.gameObject.SetActive(false);

    }
    public void SetThreatErrorPanel()
    {
       ThreatErrorPanel.gameObject.SetActive(true);

    }

    public void hideThreatErrorPanel()
    {
        ThreatErrorPanel.gameObject.SetActive(false);

    }

    public void SetGameInstructionsPanel()
    {
        GameInstructionsPanel.gameObject.SetActive(true);

    }

    public void hideGameInstructionsPanel()
    {
        GameInstructionsPanel.gameObject.SetActive(false);
    
    }
    public void cleanThreatToggles()
    {
        Threat1.GetComponentInChildren<Toggle>().isOn = false;
        Threat2.GetComponentInChildren<Toggle>().isOn = false;
        Threat3.GetComponentInChildren<Toggle>().isOn = false;
        Threat4.GetComponentInChildren<Toggle>().isOn = false;
    }

    public void cleanCorrectionsToggles()
    {
        ThreatCorrection1.GetComponentInChildren<Toggle>().isOn = false;
        ThreatCorrection2.GetComponentInChildren<Toggle>().isOn = false;
        ThreatCorrection3.GetComponentInChildren<Toggle>().isOn = false;
        ThreatCorrection4.GetComponentInChildren<Toggle>().isOn = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
