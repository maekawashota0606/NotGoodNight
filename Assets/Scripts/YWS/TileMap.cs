using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : SingletonMonoBehaviour<TileMap>
{
    //タイル収納マップ
    public GameObject[,] tileMap = new GameObject[10, 10];

    public void FindBasePoint()
    {
        //効果処理フェイズで使用カードが選択されていおり、マウスカーソルが盤面の上にいる場合
        if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.IsMouseOnTile == true)
        {
            //マウスカーソルがいるマスが探す
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tileMap[j, i].tag == "Search")
                    {
                        DecideSearchArea(j, i);
                        if (GameDirector.Instance.IsBasePointInArea == false)
                        {
                            tileMap[j, i].tag = "Untagged";
                        }
                    }
                }
            }
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
        switch(GameDirector.Instance.SelectedCard.ID)
        {
        case 1: //サラマンダーブレス
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            break;

        case 2: //ウンディーネ・ウェイブ
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;

        case 3: //シルフ・ゲイル
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;

        case 4: //ノーム・グレイブル
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;

        case 8: //彗星
            for (int i = basicPosX-1; i > -1; i--) //←
            {
                tileMap[i, basicPosZ].tag = "Area";
            }
            for (int i = basicPosX+1; i < 10; i++) //→
            {
                tileMap[i, basicPosZ].tag = "Area";
            }
            break;

        case 9: //グラビトンブレイク
            /*for (int up = basicPosZ-1; up > 0; up--) //↑
            {
                if (Map.Instance.map[basicPosX, up] == Map.Instance.meteor)
                {
                    tileMap[basicPosX, up].tag = "Search";
                    for (int upLeft = basicPosX-1; upLeft > -1; upLeft--) //↑←
                    {
                        if (Map.Instance.map[upLeft, up] == Map.Instance.meteor)
                        {
                            tileMap[upLeft, up].tag = "Search";
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int upRight = basicPosX+1; upRight < 10; upRight++) //↑→
                    {
                        if (Map.Instance.map[upRight, up] == Map.Instance.meteor)
                        {
                            tileMap[upRight, up].tag = "Search";
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }*/
            break;

        case 12: //コメットブロー
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosZ > 0 && basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ < 9)
                tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0 && basicPosZ < 9)
                tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;

        case 29: //ドラゴニックブレス
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX < 9 && basicPosZ > 1)
                tileMap[basicPosX+1, basicPosZ-2].tag = "Area"; //→↑↑
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ < 9)
                tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            break;

        case 30: //タイダルウェーブ
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX > 1 && basicPosZ > 0)
                tileMap[basicPosX-2, basicPosZ-1].tag = "Area"; //←←↑
            if (basicPosX > 0 && basicPosZ > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break; 

        case 31: //シルフ・サイクロン
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 8)
                tileMap[basicPosX, basicPosZ+2].tag = "Area"; //↓↓
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 8)
                tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;

        case 32: //テラリウムグレイブ
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX > 0 && basicPosZ > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosX > 0 && basicPosZ < 9)
                tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;

        default:
            break;
        }
    }

    /// <summary>
    /// 隕石の破壊
    /// </summary>
    public void MeteorDestory()
    {
        int DestroyedNum = 0;
        for (int z = 0; z < 10; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                if ((GameDirector.Instance.IsBasePointInArea == true && tileMap[x, z].tag == "Search") || tileMap[x, z].tag == "Area")
                {
                    //見つけた範囲内の隕石を破壊する
                    for (int targetNum = 0; targetNum < GameDirector.Instance.meteors.Count; targetNum++)
                    {
                        if (GameDirector.Instance.meteors[targetNum].transform.position.x == x && GameDirector.Instance.meteors[targetNum].transform.position.z == z * -1)
                        {
                            //隕石オブジェクトを削除する
                            Destroy(GameDirector.Instance.meteors[targetNum].gameObject);
                            //リストから削除
                            GameDirector.Instance.meteors.RemoveAt(targetNum);
                            //マップから削除
                            Map.Instance.map[z, x] = Map.Instance.empty;
                            DestroyedNum++;
                        }
                    }
                    tileMap[x, z].tag = "Untagged";
                }
            }
        }
        if (DestroyedNum != 0)
        {
            GameDirector.Instance.AddScore(DestroyedNum);
            GameDirector.Instance.IsMeteorDestroyed = true;
            if (GameDirector.Instance.IsMultiEffect == false)
            {
                GameDirector.Instance.IsPlayerSelectMove = true;
            }
        }
    }

    /// <summary>
    /// 全てのタイルのタグを初期化する
    /// </summary>
    public void ResetTileTag()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                tileMap[j,i].tag = "Untagged";
            }
        }
    }
}