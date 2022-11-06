using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pausePopup;//일시정지 팝업
    [SerializeField] private GameObject bgmButton;
    [SerializeField] private GameObject effectButton;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject quitButton;
    private bool bgmOnOff = true;
    private bool soundOnOff = true;
    private bool isdelegated = false;
    //private void Awake()
    //{
    //    Init();
    //}
    public void Init()
    {
        canvas = GameObject.Find("Canvas");
        
        pausePopup = canvas.transform.GetChild(3).gameObject;
        pauseButton = canvas.transform.GetChild(0).GetChild(3).GetChild(1).gameObject;
        bgmButton = pausePopup.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetChild(1).GetChild(1).gameObject;
        effectButton = pausePopup.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).GetChild(1).GetChild(1).gameObject;
        replayButton = pausePopup.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(1).gameObject;
        quitButton = pausePopup.transform.GetChild(0).GetChild(2).GetChild(2).GetChild(0).gameObject;

        pauseButton.GetComponent<Button>().onClick.AddListener(delegate { Pause(); });
        
    }
    public void Pause()//일시정지 on/off
    {
        if (!pausePopup.activeSelf)
        {
            pausePopup.SetActive(true);

            if (!isdelegated)
            {
                DelegateEvent();
            }
        }
        else
        {
            pausePopup.SetActive(false);
        }

    }

    private void DelegateEvent()
    {
        bgmButton.GetComponentInParent<Button>().onClick.AddListener(delegate { ClickBGM(); });
        effectButton.GetComponentInParent<Button>().onClick.AddListener(delegate { ClickEffect(); });
        replayButton.GetComponent<Button>().onClick.AddListener(delegate { Quit(); });
        quitButton.GetComponent<Button>().onClick.AddListener(delegate { Replay(); });
        isdelegated = true;
    }

    public void Quit()//게임플레이->메인화면
    {
        SceneManager.LoadScene("Main");
        ProgramManager.Init();
    }
    public void Replay()
    {
        SceneManager.LoadScene("ToTestGrid");
        isdelegated = false;
    }
    public void ClickBGM()
    {
        if (bgmOnOff)
        {
            ProgramManager.Sound.Clear(Define.Sound.BGM);
        }
        else
        {
            ProgramManager.Sound.Play("BGM_02", Define.Sound.BGM);
        }
        SoundOnOff(Define.Sound.BGM, ref bgmOnOff);
        
    }
    public void ClickEffect()
    {
        SoundOnOff(Define.Sound.Effect, ref soundOnOff);
    }
    private void SoundOnOff(Define.Sound type, ref bool onOff)
    {
        GameObject button;
        if (type == Define.Sound.BGM)
            button = bgmButton;
        else
            button = effectButton;
        if (onOff)//켜져있다->끈다
        {
            button.transform.localPosition = new Vector3(0, 0, 0);
        }
        else//꺼져있다->킨다
        {
            button.transform.localPosition = new Vector3(201f, 0, 0);
        }
        onOff = !(onOff);
    }
}
