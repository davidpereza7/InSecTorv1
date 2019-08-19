using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour
{
    public RoundData roundData = new RoundData();
    public Text playerName;
    void OnDisable()
    {
         PlayerPrefs.SetString("playername", roundData.name);
    }
    public void registerPlayerName()
    {
        roundData.name = playerName.text.ToString();
        Debug.Log("el nombre del jugador es " + roundData.name);
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
        SceneManager.LoadScene("Home");
    }
}