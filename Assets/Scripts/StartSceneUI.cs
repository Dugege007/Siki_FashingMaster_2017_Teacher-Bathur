using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneUI : MonoBehaviour
{
    public void NewGame()
    {
        PlayerPrefs.DeleteKey("gold");
        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("exp");
        PlayerPrefs.DeleteKey("smallCountdown");
        PlayerPrefs.DeleteKey("bigCountdown");
        SceneManager.LoadScene(1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
