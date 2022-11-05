using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GridSettingValue : MonoBehaviour
{
    public bool IsDebug = true;
    public GameObject TileObject,BoardObject;
    public Sprite RAINBOW;
    public List<Sprite> spriteList;

    public List<grid_Value> inputList;
    
    public int getInput(string tag)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            if (inputList[i].tag == tag)
                return inputList[i].input;
        }

        Debug.Log("error Tag - " + tag);
        return -1;
    }
}

[System.Serializable]
public class grid_Value
{
    public string tag;
    public int input;
}