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

        if (GameDirector.Instance.IsMouseLeaveTile == true)
        {
            ResetTileTag();
            GameDirector.Instance.IsMouseLeaveTile = false;
        }

        if (GameDirector.Instance.NeedSearch == true)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if ((GameDirector.Instance.IsBasePointInArea == true && map[j, i].tag == "Search") || map[j, i].tag == "Area")
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
        //カードの種類に基づいて、範囲となるマスすべてにタグを付ける
        switch(GameDirector.Instance.SelectedCardNum)
        {
        case 1: //サラマンダーブレス
            if (basicPosZ > 1)
                map[basicPosX, basicPosZ-2].tag = "Area";
            if (basicPosZ != 0)
                map[basicPosX, basicPosZ-1].tag = "Area";
            if (basicPosZ != 9)
                map[basicPosX, basicPosZ+1].tag = "Area";
            break;

        case 2: //ウンディーネ・ウェイブ
            if (basicPosX > 1)
                map[basicPosX-2, basicPosZ].tag = "Area";
            if (basicPosX != 0)
                map[basicPosX-1, basicPosZ].tag = "Area";
            if (basicPosX != 9)
                map[basicPosX+1, basicPosZ].tag = "Area";
            break;

        case 3: //シルフ・ゲイル
            GameDirector.Instance.IsBasePointInArea = false;
            if (basicPosZ != 0)
                map[basicPosX, basicPosZ-1].tag = "Area";
            if (basicPosZ != 9)
                map[basicPosX, basicPosZ+1].tag = "Area";
            if (basicPosX != 0)
                map[basicPosX-1, basicPosZ].tag = "Area";
            if (basicPosX != 9)
                map[basicPosX+1, basicPosZ].tag = "Area";
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