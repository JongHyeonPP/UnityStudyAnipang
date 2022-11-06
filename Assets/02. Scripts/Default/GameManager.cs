using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //private void Awake()
    //{
    //    Init();
    //}
    public bool gameover;
    private int Score { get; set; }//점수
    public Image timerFill;
    public void Init()//게임 매니저 초기화
    {
            gameover = false;
            Score = 0;      
    }
}
