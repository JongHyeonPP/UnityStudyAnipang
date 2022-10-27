using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;//싱글턴 구현, 다른 오브젝트에서 호출 시 Managers mg = Managers.GetInstance();
    public bool gameover;
    public static GameManager GetInstance() { return instance; }
    private int Score { get; set; }//점수
    private float remainTime = 60;
    public Image timerFill;
    void Start()
    {
        Init();
        StartCoroutine(StartTimer());
    }
    void Init()//게임 매니저 초기화
    {
        if (instance == null)//인스턴스 배정이 돼있는가
        {
            GameObject temp = GameObject.Find("GameManager");//GameManager라는 이름의 오브젝트를 Find
            if (temp == null)//못 찾았다면
            {
                temp = new GameObject { name = "GameManager" };//go에 
            }
            if (temp.GetComponent<GameManager>() == null)//오브젝트는 가져왔지만 GameManager 컴포넌트가 없는 경우
            {
                temp.AddComponent<GameManager>();//컴포넌트만 추가한다
            }
            DontDestroyOnLoad(temp);//씬 이동해도 사라지지 않도록 한다
            gameover = false;
            Score = 0;
            remainTime = 60;
        }
    }
    IEnumerator StartTimer()
    {
        while(true)
        {
            timerFill.fillAmount = remainTime / 60f;
            if (remainTime >= 0)
            {
                remainTime -= 0.1f;
            }
            else
            {
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
