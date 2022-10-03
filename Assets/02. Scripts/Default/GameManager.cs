using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;//싱글턴 구현, 다른 오브젝트에서 호출 시 Managers mg = Managers.GetInstance();
    public bool gameover;
    public static GameManager GetInstance() { return instance; }
    private int Score { get; set; }//점수
    public struct item//아이템 개수 구조체
    {
        public int bomb, potion, elec;//폭탄, 물약, 전기
        public void Init()
        {
            bomb = potion = elec = 1;//초기값 1
        }
    }
    void Start()
    {
        Init();   
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
        }
    }
}
