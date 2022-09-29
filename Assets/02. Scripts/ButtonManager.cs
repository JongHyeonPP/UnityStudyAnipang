using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public GameObject pausePopup;//일시정지 팝업
    private bool isPaused = false;
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
}
