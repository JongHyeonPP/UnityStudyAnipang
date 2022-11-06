using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgramManager : MonoBehaviour
{
    public static ProgramManager p_instance;
    static ProgramManager Instance { get { Init(); return p_instance; } }
    private SoundManager soundManager;
    private UIManager uiManager;
    private GameManager gameManager; 
    public static SoundManager Sound { get { return Instance.soundManager; } }
    public static UIManager UI { get { return Instance.uiManager; } }
    public static GameManager Game { get { return Instance.gameManager; } }
    private void Awake()
    {
        Init();
        Sound.Init();
        Game.Init();
        UI.Init();
    }
    public static void Init()
    {
        // Instance 프로퍼티 get 시 호출되니까 또 여기서 Instance 쓰면 무한 루프 빠짐 주의
        if (p_instance == null)
        {
            GameObject p_object = GameObject.Find("ProgramManager");
            if (p_object == null)
            {
                p_object = new GameObject("ProgramManager");
                p_object.AddComponent<ProgramManager>();
            }

            DontDestroyOnLoad(p_object);
            p_instance = p_object.GetComponent<ProgramManager>();

            
        }
        if (p_instance.soundManager == null)
        {
            p_instance.soundManager = Instantiate(new GameObject("SoundManager")).AddComponent<SoundManager>();
            p_instance.soundManager.gameObject.transform.parent = p_instance.transform;
        }
        if (p_instance.uiManager == null)
        {
            p_instance.uiManager = Instantiate(new GameObject("UIManager")).AddComponent<UIManager>();
            p_instance.uiManager.gameObject.transform.parent = p_instance.transform;
        }
        if (p_instance.gameManager == null)
        {
            p_instance.gameManager = Instantiate(new GameObject("GameManager")).AddComponent<GameManager>();
            p_instance.gameManager.gameObject.transform.parent = p_instance.transform;
        }
        
    }

    public void Clear()
    {
        
    }
}