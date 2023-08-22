using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    public GameObject settingPanel;
    public Toggle muteToggle;

    private void Start()
    {
        muteToggle.isOn = !AudioManager.Instance.IsMute;
    }

    public void SwitchMute(bool isOn)
    {
        AudioManager.Instance.SwitchMuteState(isOn);
    }

    public void OnBackButtonDown()
    {
        // Save Game
        PlayerPrefs.SetInt("gold", GameController.Instance.gold);
        PlayerPrefs.SetInt("level", GameController.Instance.level);
        PlayerPrefs.SetInt("exp", GameController.Instance.exp);
        PlayerPrefs.SetFloat("smallCountdown", GameController.Instance.smallTimer);
        PlayerPrefs.SetFloat("bigCountdown", GameController.Instance.bigTimer);
        int isMute = (AudioManager.Instance.IsMute == false) ? 0 : 1;
        PlayerPrefs.SetInt("mute", isMute);
        SceneManager.LoadScene(0);
    }

    public void OnSettingButtonDown()
    {
        settingPanel.SetActive(true);
    }

    public void OnCloseButtonDown()
    {
        settingPanel.SetActive(false);
    }
}
