using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    public AudioSource bgmAudioSource;
    public AudioClip seaWaveClip;
    public AudioClip goldClip;
    public AudioClip rewardClip;
    public AudioClip fireClip;
    public AudioClip changeClip;
    public AudioClip levelUpClip;

    private bool isMute = false;
    public bool IsMute
    {
        get { return isMute; }
    }

    private void Awake()
    {
        instance = this;
        isMute = (PlayerPrefs.GetInt("mute", 0) == 0) ? false : true;
        DoMute();
    }

    public void SwitchMuteState(bool isOn)
    {
        isMute = !isOn;
        DoMute();
    }

    private void DoMute()
    {
        if (isMute)
            bgmAudioSource.Pause();
        else
            bgmAudioSource.Play();
    }

    public void PlayEffectSound(AudioClip clip)
    {
        if (!isMute)
            AudioSource.PlayClipAtPoint(clip, Vector3.zero, 1f);
    }
}
