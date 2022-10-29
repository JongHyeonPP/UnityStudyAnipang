using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GridManager : MonoBehaviour
{
    public GridTable gt;
    public _GridSettingValue values;

    public void tempBtn_New()
    {
        Transform[] childList = values.BoardObject.GetComponentsInChildren<Transform>();
        for (int i = 1; i < childList.Length; i++)
        {
            Destroy(childList[i].gameObject);
        }

        gt = new GridTable(this);

        GridLayoutGroup temp = values.BoardObject.GetComponent<GridLayoutGroup>();
        Vector2 tempV2 =  values.BoardObject.GetComponent<RectTransform>().sizeDelta;
        temp.cellSize = new Vector2( (tempV2.x/ 10), tempV2.y / 10);

        gt.newGrid(values.getInput("Board_x"), values.getInput("Board_y"));
    }

    public void tempBtn_TryMove()
    {
        Tile target1 = gt.gridTable[values.getInput("Target1_x"), values.getInput("Target1_y")];
        Tile target2 = gt.gridTable[values.getInput("Target2_x"), values.getInput("Target2_y")];

        List<Tile> list = new List<Tile>();
        List<Vector2> score = new List<Vector2>();

        gt.swapTile(target1, target2);

        gt.checkTile_AnsAll(ref list, ref score);

        if (list.Count == 0)
        {
            gt.swapTile(target1, target2);
            return;
        }


        for (int i = 0; i < list.Count; i++)
        {
            gt.fallingNode(list[i]);
        }

        for (int i = 0; i < list.Count; i++)
        {
            list[i].set_Random(this);
        }
    }

    public SpriteRenderer instanceTile()
    {
        return Instantiate(values.TileObject, values.BoardObject.transform).GetComponent<SpriteRenderer>();
    }
}

public class GridTable
{
    int sizeX, sizeY;
    public Tile[,] gridTable;
    GridManager gm;

    public GridTable(GridManager _gm)
    {
        gm = _gm;
    }

    public void newGrid(int _sizeX, int _sizeY)
    {
        sizeX = _sizeX;
        sizeY = _sizeY;
        gridTable = new Tile[sizeX, sizeY];
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                gridTable[x, y] = new Tile();
                gridTable[x, y].sprite = gm.instanceTile();
                gridTable[x, y].set_Random(gm,new Vector2(x,y));
                gridTable[x, y].sprite.transform.name += ", " + x + " / " + y;
            }
        }
    }

    public void fallingNode(Tile targetTile)
    {
        Debug.Log(targetTile.sprite.name);
        for (int y = (int)targetTile.posV2.y-1 ; y >= 0; y--)
        {
            Debug.Log((int)targetTile.posV2.x + " / " + y);
            swapTile(gridTable[(int)targetTile.posV2.x, y], gridTable[(int)targetTile.posV2.x, y+1]);
        }
    }


    public bool TryMove(Tile target_1, Tile target_2, ref List<Tile> list, ref List<Vector2> score)
    {
        swapTile(target_1, target_2);

        checkTile_AnsAll(ref list, ref score);

        if (list.Count == 0)
        {
            return false;
        }

        return true;
    }

    public void swapTile(Tile target_1, Tile target_2)
    {
        {
            Vector2 temp = target_1.posV2;
            target_1.posV2 = target_2.posV2;
            target_2.posV2 = temp;
        }

        {
            string name_1 = target_1.sprite.transform.name;
            string name_2 = target_2.sprite.transform.name;

            target_1.sprite.transform.name = name_2;
            target_2.sprite.transform.name = name_1;

            Tile temp = gridTable[(int)target_1.posV2.x, (int)target_1.posV2.y];
            gridTable[(int)target_1.posV2.x, (int)target_1.posV2.y] = gridTable[(int)target_2.posV2.x, (int)target_2.posV2.y];
            gridTable[(int)target_2.posV2.x, (int)target_2.posV2.y] = temp;

        }

        {
            int temp1 = target_1.sprite.transform.GetSiblingIndex();
            int temp2 = target_2.sprite.transform.GetSiblingIndex();

            target_1.sprite.transform.SetSiblingIndex(temp2);
            target_2.sprite.transform.SetSiblingIndex(temp1);
        }
    }

    void tileInterAction(int x, int y)
    {
        ereaseTile(x, y);
    }

    void createTile(ref List<Tile> emptyList)
    {
        for (int i = 0; i < emptyList.Count; i++)
        {
            int x = (int)emptyList[i].posV2.x;
            int y = (int)emptyList[i].posV2.y;

            gridTable[x,y].set_Random(gm, new Vector2(x,y));
        }
    }

    int checkTile_Pos(int pos_x,int pos_y)
    {
        int myType = gridTable[pos_x, pos_y].type;
        List<Vector2> list_Cross = new List<Vector2>();
        List<Vector2> list_Plus = new List<Vector2>();

        for (int y = -1; y < 2; y++)
        {
            for (int x = -1; x < 2; x++)
            {
                if (isInBoard(pos_x + x, pos_y + y))
                {
                    if (myType == gridTable[pos_x + x, pos_y + y].type)
                    {
                        if(!(x == 0 && y==0))
                        {
                            if (x == 0 || y == 0)
                            {
                                list_Plus.Add(new Vector2(x, y));
                            }
                            else
                            {
                                list_Cross.Add(new Vector2(x, y));
                            }
                        }
                    }
                }
            }
        }

        if(list_Plus.Count > 1)
        {
            for (int std = 0; std < list_Plus.Count; std++)
            {
                for (int target = std + 1; target < list_Plus.Count; target++)
                {
                    bool x = (list_Plus[std].x * list_Plus[target].x == 0);
                    bool y = (list_Plus[std].y * list_Plus[target].y == 0);

                    if (!(x && y))
                        return 1;
                }
            }
        }

        if (list_Cross.Count > 0)
        {
            for (int std = 0; std < list_Cross.Count; std++)
            {
                for (int target = std + 1; target < list_Cross.Count; target++)
                {
                    int check_1 = (int)(list_Cross[std].x * list_Cross[target].x);
                    int check_2 = (int)(list_Cross[std].y * list_Cross[target].y);

                    if (check_1 != check_2)
                        return 2;

                }

                for (int target = 0; target < list_Plus.Count; target++)
                {

                    int check_1 = (int)(list_Cross[std].x * list_Plus[target].x);
                    int check_2 = (int)(list_Cross[std].y * list_Plus[target].y);

                    if (check_1 + check_2 == -1)
                        gridTable[pos_x, pos_y].sprite.transform.name += "2";
                }
            }
        }

        if (list_Plus.Count > 0)
        {
            for (int std = 0; std < list_Plus.Count; std++)
            {
                int x = pos_x + (int)(list_Plus[std].x * -2);
                int y = pos_y + (int)(list_Plus[std].y * -2);

                if (isInBoard(x, y))
                {
                    if (gridTable[x, y].type == myType)
                    {
                        return 2;
                    }    
                }
            }
        }

        return 0;
    }

    int checkTile_Ans(Tile targetTile, Vector2 std_V2)
    {
        int std = targetTile.type;
        int returnValue = 1;
        while (true)
        {
            Vector2 temp = std_V2 * returnValue;
            int x = (int)(targetTile.posV2 + temp).x;
            int y = (int)(targetTile.posV2 + temp).y;
            if (isInBoard(x,y))
            {
                if(std  == gridTable[x,y].type)
                    returnValue++;
                else
                    break;  
            }
            else
                break;
        }

        return returnValue - 1;
    }

    public bool checkTile_AnsAll(ref List<Tile> list, ref List<Vector2> score)
    {
        Vector2 temp;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                temp = Vector2.zero;
                temp.x = checkTile_Ans(gridTable[x,y],Vector2.right) + checkTile_Ans(gridTable[x, y], Vector2.left);
                temp.y = checkTile_Ans(gridTable[x, y], Vector2.up) + checkTile_Ans(gridTable[x, y], Vector2.down);

                if (temp.x <= 1)
                    temp.x = 0;

                if (temp.y <= 1)
                    temp.y = 0;

                if(temp != Vector2.zero)
                {
                    list.Add(gridTable[x, y]);
                    score.Add(temp);
                }
            }
        }

        return list.Count > 0;
    }

    bool isInBoard(int x,int y)
    {
        if (x < 0)
            return false;

        if (x >= sizeX)
            return false;

        if (y < 0)
            return false;

        if (y >= sizeX)
            return false;

        return true;
    }

    void ereaseTile(int x, int y)
    {
        Tile temp = gridTable[x, y];
        gridTable[x, y].sprite.sprite = null;
        //gridTable[x, y] = null;
    }
}

public class Tile
{
    GridManager gm;
    public SpriteRenderer sprite;
    public int type;
    public Vector2 posV2;
    public int ability;

    public void set_Value(int _type, Vector2 _posV2, int _ability)
    {
        type = _type;
        posV2 = _posV2;
        ability = _ability;
    }

    public void set_Tile(Tile target)
    {
        type = target.type;
        posV2 = target.posV2;
    }

    public void set_Random(GridManager _gm, Vector2 _posV2)
    {
        gm = _gm;

        posV2 = _posV2;

        int rand = Random.Range(0, gm.values.spriteList.Count);
        sprite.sprite = gm.values.spriteList[rand];
        sprite.size = new Vector2(1, 1);
        type = rand;

        ability = 2;
    }

    public void set_Random(GridManager _gm)
    {
        gm = _gm;

        int rand = Random.Range(0, gm.values.spriteList.Count);
        sprite.sprite = gm.values.spriteList[rand];
        sprite.size = new Vector2(1, 1);
        type = rand;

        ability = 2;
    }
}

