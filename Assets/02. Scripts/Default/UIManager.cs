using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour, IManager
{
    public GameObject pausePopup;//일시정지 팝업
    private bool isPaused = false;
    public GameObject musicButton;
    public GameObject soundButton;
    public bool musicOnOff = true;
    public bool soundOnOff = true;
    public AudioSource audioSource;
    public void Pause()//일시정지 on/off
    {
        if (!isPaused)
        {
            pausePopup.SetActive(true);
        }
        else
        {
            pausePopup.SetActive(false);
        }

    }
    public void Quit()//게임플레이->메인화면
    {
        SceneManager.LoadScene("Main");
    }
    public void Replay()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void ClickMusic()
    {
        SoundOnOff(musicButton, ref musicOnOff);
        audioSource.mute = !musicOnOff;
    }
    public void ClickSound()
    {
        SoundOnOff(soundButton, ref soundOnOff);
    }
    private void SoundOnOff(GameObject button, ref bool onOff)
    {
        if (onOff)//켜져있다->끈다
        {
            button.transform.localPosition = new Vector3(-201f, 0, 0);
        }
        else//꺼져있다->킨다
        {
            button.transform.localPosition = new Vector3(0, 0, 0);
        }
        onOff = !(onOff);
    }
    public void Init()
    {
    
    }

    public void Clear()
    {
        throw new System.NotImplementedException();
    }
}
