using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR;
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
        Vector2 tempV2 = values.BoardObject.GetComponent<RectTransform>().sizeDelta;
        temp.cellSize = new Vector2((tempV2.x / 10), tempV2.y / 10);

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

        for (int i = 0; i < list.Count; i++)
        {
            list[i].sprite.name += score[i];
        }

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
    public void Item_BOMB()
    {
        Tile target1 = gt.gridTable[values.getInput("Target2_x"), values.getInput("Target2_y")];

        List<Tile> list = new List<Tile>();
        gt.searchTile_range(target1, ref list);


        if (values.IsDebug)
            for (int i = 0; i < list.Count; i++)
            {
                list[i].sprite.sprite = values.RAINBOW;
            }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                gt.fallingNode(list[i]);
            }

            for (int i = 0; i < list.Count; i++)
            {
                list[i].set_Random(this);
            }
        }
    }
    public void Item_RAINBOW()
    {
        Tile target1 = gt.gridTable[values.getInput("Target1_x"), values.getInput("Target1_y")];
        List<Tile> list = new List<Tile>();
        gt.searchTile_type(target1.type, ref list);

        for (int i = 0; i < list.Count; i++) { 
            list[i].sprite.sprite = values.RAINBOW;
            list[i].type = -1;
        }
    }
    public void Item_SPARK()
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
                gridTable[x, y].set_Random(gm, new Vector2(x, y));
                gridTable[x, y].sprite.transform.name += ", " + x + " / " + y;
            }
        }
    }

    public void fallingNode(Tile targetTile)
    {
        Debug.Log(targetTile.sprite.name);
        for (int y = (int)targetTile.posV2.y - 1; y >= 0; y--)
        {
            Debug.Log((int)targetTile.posV2.x + " / " + y);
            swapTile(gridTable[(int)targetTile.posV2.x, y], gridTable[(int)targetTile.posV2.x, y + 1]);
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


    public void searchTile_type(int targetType, ref List<Tile> list)
    {
        int std = targetType;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if(gridTable[x, y].isTileSame(targetType))
                    list.Add(gridTable[x, y]);
            }
        }
    }
    public void searchTile_range(Tile targetTile, ref List<Tile> list)
    {
        int target_x;
        int target_y;

        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                target_x = (int)targetTile.posV2.x + x;
                target_y = (int)targetTile.posV2.y + y;
                if (isInBoard(target_x, target_y))
                    if (x*x *y*y != 16)
                        list.Add(gridTable[target_x, target_y]);
            }
        }
    }


    int checkTile_Ans(Tile targetTile, Vector2 std_V2)
    {
        int std = targetTile.type;
        int returnValue = 1;
        int fix = 1;
        if (std == -1)
        {
            while (true)
            {
                Vector2 temp = std_V2 * returnValue;
                int x = (int)(targetTile.posV2 + temp).x;
                int y = (int)(targetTile.posV2 + temp).y;
                if (isInBoard(x, y))
                {
                    if (gridTable[x, y].isTileSame(std))
                        returnValue++;
                    else
                    {
                        std = gridTable[x, y].type;
                        break;
                    }
                }
                else
                    break;
            }
        }


        while (true)
        {
            Vector2 temp = std_V2 * returnValue;
            int x = (int)(targetTile.posV2 + temp).x;
            int y = (int)(targetTile.posV2 + temp).y;
            if (isInBoard(x, y))
            {
                if (gridTable[x, y].isTileSame(std))
                    returnValue++;
                else
                    break;
            }
            else
                break;
        }

        if (targetTile.type == -1)
        {
            if (std != -1)
            {
                int opp_x = (int)(targetTile.posV2 + std_V2 * -1).x;
                int opp_y = (int)(targetTile.posV2 + std_V2 * -1).y;
                if (isInBoard(opp_x, opp_y))
                {
                    Tile Tile_opp = gridTable[opp_x, opp_y];

                    if (!Tile_opp.isTileSame(std))
                        if(returnValue == 2)
                            fix = 2;
                }
            }
        }

        return returnValue - fix;
    }

    public bool checkTile_AnsAll(ref List<Tile> list, ref List<Vector2> score)
    {
        Vector2 temp;

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                temp = Vector2.zero;
                temp.x = checkTile_Ans(gridTable[x, y], Vector2.right) + checkTile_Ans(gridTable[x, y], Vector2.left);
                temp.y = checkTile_Ans(gridTable[x, y], Vector2.up) + checkTile_Ans(gridTable[x, y], Vector2.down);

                if (temp.x <= 1)
                    temp.x = 0;

                if (temp.y <= 1)
                    temp.y = 0;

                if (temp != Vector2.zero)
                {
                    list.Add(gridTable[x, y]);
                    score.Add(temp);
                }
            }
        }

        return list.Count > 0;
    }

    bool isInBoard(int x, int y)
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

    public void set_Value(int _type, Vector2 _posV2)
    {
        type = _type;
        posV2 = _posV2;
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
    }

    public void set_Random(GridManager _gm)
    {
        gm = _gm;

        int rand = Random.Range(0, gm.values.spriteList.Count);
        sprite.sprite = gm.values.spriteList[rand];
        sprite.size = new Vector2(1, 1);
        type = rand;
    }

    public bool isTileSame(int comp)
    {
        if (type == -1)
            return true;

        if (type == comp)
            return true;

        return false;
    }
}

