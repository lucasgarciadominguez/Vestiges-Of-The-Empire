using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource m_audioBGMusic;
    [SerializeField]
    AudioSource m_audioClickUIInspector;
    [SerializeField]
    AudioSource m_audioCloseUIInspectors;
    [SerializeField]
    AudioSource m_audioOpenUIInspectors;
    [SerializeField]
    AudioSource m_audioButtonClick;
    [SerializeField]
    AudioSource m_audioConstructionPlaced;
    [SerializeField]
    AudioSource m_audioNotification;
    public void ClickInInspector()
    {
        m_audioClickUIInspector.Play();
    }
    public void OpenInspector()
    {
        m_audioOpenUIInspectors.Play();
    }
    public void CloseInspector()
    {
        m_audioCloseUIInspectors.Play();
    }
    public void PauseMusicBG()
    {
        m_audioBGMusic.Pause();
    }
    public void ContinueMusicBG()
    {
        m_audioBGMusic.Play();
    }
    public void ClickButton()
    {
        m_audioButtonClick.Play();
    }
    public void ConstructionFinishedAndPlaced()
    {
        m_audioConstructionPlaced.Play();
    }
    public void NewAdvise()
    {
        m_audioNotification.Play();
    }
}
