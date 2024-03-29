using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : SingletonMonoBehaviour<TileMap>
{
    //タイル収納マップ
    public GameObject[,] tileMap = new GameObject[10, 10];
    //グラビトンブレイク用の座標保存リスト
    public List<int> checkListX = new List<int>();
    public List<int> checkListZ = new List<int>();

    /// <summary>
    /// カードの効果範囲の原点を探す関数
    /// </summary>
    public void FindBasePoint()
    {
        //使用カードが選択されていおり、マウスカーソルが盤面の上にいる場合
        if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.IsMouseOnTile == true)
        {
            //マウスカーソルがいるマスが探す
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (tileMap[j, i].CompareTag("Search"))
                    {
                        if (GameDirector.Instance.SelectedCard.ID == 9)
                        {
                            checkListX = new List<int>();
                            checkListZ = new List<int>();
                            checkListX.Add(j);
                            checkListZ.Add(i);
                            Debug.Log(checkListX.Count + " " + checkListZ.Count);
                        }
                        //使用カードによって範囲を表示する
                        GameDirector.Instance.SelectedCard.DecideSearchArea(j, i);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 隕石の破壊
    /// </summary>
    public void MeteorDestory()
    {
        for (int z = 0; z < 10; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (tileMap[x, z].CompareTag("Search") || tileMap[x, z].CompareTag("Area"))
                {
                    //見つけた範囲内の隕石を破壊する
                    for (int targetNum = 0; targetNum < GameDirector.Instance.meteors.Count; targetNum++)
                    {
                        if (GameDirector.Instance.meteors[targetNum].transform.position.x == x && GameDirector.Instance.meteors[targetNum].transform.position.z == z * -1)
                        {
                            SoundManager.Instance.PlaySE(6);
                            if (GameDirector.Instance.SelectedCard.ID == 9)
                            {
                                checkListX.Add(x);
                                checkListZ.Add(z);
                            }
                            //隕石オブジェクトを削除する
                            Destroy(GameDirector.Instance.meteors[targetNum].gameObject);
                            //リストから削除
                            GameDirector.Instance.meteors.RemoveAt(targetNum);
                            //マップから削除
                            Map.Instance.map[z, x] = Map.Instance.empty;
                            GameDirector.Instance.DestroyedNum++;
                        }
                    }
                    tileMap[x, z].tag = "Untagged";
                }
            }
        }
        if (GameDirector.Instance.DestroyedNum != 0)
        {
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