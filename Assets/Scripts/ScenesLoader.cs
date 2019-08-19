using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{

    public void LoadSceneRouter()
    {
        SceneManager.LoadScene("RouterThreads");
    }
    public void LoadSceneSmartTV()
    {
        SceneManager.LoadScene("SmartTV");
    }

    public void nextLevelInHome()
    {
        SceneManager.UnloadSceneAsync("MinimumReached");
        SceneManager.LoadScene("Home1");
    }

}
