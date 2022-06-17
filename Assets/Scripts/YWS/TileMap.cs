using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : SingletonMonoBehaviour<TileMap>
{
    public GameObject[,] map = new GameObject[10, 10];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが行動可能、かつ使用カードが選択されていおり、マウスカーソルが盤面の上にいる場合
        if (GameDirector.Instance.CanPlayerControl == true && GameDirector.Instance.IsCardSelect == true && GameDirector.Instance.IsTileNeedSearch == true)
        {
            //マウスカーソルがいるマスが探す
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[j, i].tag == "Search")
                    {
                        DecideSearchArea(j, i);
                    }
                }
            }
        }

        if (GameDirector.Instance.IsMouseLeftTile == true)
        {
            ResetTileTag();
            GameDirector.Instance.IsMouseLeftTile = false;
        }

        if (GameDirector.Instance.NeedSearch == true)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[j, i].tag == "Search" || map[j, i].tag == "Area")
                    {
                        Debug.Log("hit" + map[j, i].name);
                        GameDirector.Instance.MeteorDestory(j, i);
                        map[j, i].tag = "Untagged";
                    }
                }
            }
            GameDirector.Instance.NeedSearch = false;
        }
    }

    /// <summary>
    /// カード効果の範囲を決める
    /// </summary>
    /// <param name="basicPosX">原点となるマスのx座標</param>
    /// <param name="basicPosZ">原点となるマスのz座標</param>
    public void DecideSearchArea(int basicPosX, int basicPosZ)
    {
        switch(GameDirector.Instance.SelectedCardNum)
        {
        case 1: //サラマンダーブレス
        /*
        □原点 o範囲 x範囲外
        xxxxxx
        xxxoxx
        xxxoxx
        xxx□xx
        xxxoxx
        xxxxxx
        */
        //範囲となるマスすべてにタグを付ける
        if (basicPosZ > 1)
            map[basicPosX, basicPosZ-2].tag = "Area";
        if (basicPosZ != 0)
            map[basicPosX, basicPosZ-1].tag = "Area";
        if (basicPosZ != 9)
            map[basicPosX, basicPosZ+1].tag = "Area";
        break;

        default:
            break;
        }
    }

    public void ResetTileTag()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                map[j,i].tag = "Untagged";
            }
        }
    }
}