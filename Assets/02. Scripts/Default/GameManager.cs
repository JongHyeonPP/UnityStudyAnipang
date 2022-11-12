using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
<<<<<<< HEAD
    static GameManager instance;//싱글턴 구현, 다른 오브젝트에서 호출 시 Managers mg = Managers.GetInstance();
    public GridManager grid_M;
=======
    //private void Awake()
    //{
    //    Init();
    //}
>>>>>>> origin/main
    public bool gameover;
    private int Score { get; set; }//점수
    public Image timerFill;
    public void Init()//게임 매니저 초기화
    {
<<<<<<< HEAD
        Init();
        //StartCoroutine(StartTimer());

        if (grid_M == null)
        {
            grid_M = new GameObject("GridManager").AddComponent<GridManager>();
            grid_M.gameObject.transform.parent = transform;
        }
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
        while (true)
        {
            timerFill.fillAmount = remainTime / 60f;
            if (remainTime >= 0)
            {
                Debug.Log(timerFill.fillAmount);
                remainTime -= 0.1f;
            }
            else
            {
                Debug.Log("else");
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
=======
            gameover = false;
            Score = 0;      
>>>>>>> origin/main
    }

    public int itemMode = 0;
    public GameObject target;
    public GameObject target_swap;
    private void Update()
    {
        checkTouch();
    }

    void checkTouch()
    {
        if (Camera.main == null)
        {
            Debug.Log("main cam tag is null");
            return;
        }

        Vector3 tempMousePos;
        if (Input.GetMouseButtonDown(0))
        {
            tempMousePos = Input.mousePosition;

            // V2 pos = 카메라 스크린 좌표 ( 마우스 클릭 위치 ).
            Vector2 pos = Camera.main.ScreenToWorldPoint(tempMousePos);

            // 0,0 -> pos RayCast, collider 검출
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                    target = hit.transform.gameObject;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            tempMousePos = Input.mousePosition;

            // V2 pos = 카메라 스크린 좌표 ( 마우스 클릭 위치 ).
            Vector2 pos = Camera.main.ScreenToWorldPoint(tempMousePos);

            // 0,0 -> pos RayCast, collider 검출
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Tile")
                {
                    if (target == hit.transform.gameObject)
                    {
                        if(itemMode == 0)
                        {
                            if (target_swap == null)
                            {
                                target_swap = target;
                                target_swap.transform.localScale *= 0.9f;
                                target = null;
                            }
                            else
                            {
                                if (target_swap == target)
                                {
                                     return;
                                }

                                string[] temp0 = target_swap.transform.name.Split(',');
                                string[] temp1 = target.transform.name.Split(',');

                                grid_M.tempBtn_TryMove(int.Parse(temp0[0]) , int.Parse(temp0[1]),
                                    int.Parse(temp1[0]), int.Parse(temp1[1]));

                                target_swap.transform.localScale *= 1.1f;

                                target_swap = null;
                                target = null;
                            }
                        }
                    }
                }
            }
            else
                target = null;
        }
    }
}