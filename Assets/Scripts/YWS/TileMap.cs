using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : SingletonMonoBehaviour<TileMap>
{
    //タイル収納マップ
    public GameObject[,] tileMap = new GameObject[10, 10];
    public List<int> checkListX = new List<int>();
    public List<int> checkListZ = new List<int>();

    /// <summary>
    /// カードの効果範囲の原点を探す関数
    /// </summary>
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
        #region サラマンダーブレス
        case 1:
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            break;
        #endregion

        #region ウンディーネ・ウェイブ
        case 2:
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region シルフ・ゲイル
        case 3:
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region ノーム・グレイブル
        case 4:
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;
        #endregion

        #region 星磁力
        case 6:

            break;
        #endregion

        #region グラビトンコア
        case 7:
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
        #endregion

        #region 彗星
        case 8:
            for (int i = basicPosX-1; i > -1; i--) //←
            {
                tileMap[i, basicPosZ].tag = "Area";
            }
            for (int i = basicPosX+1; i < 10; i++) //→
            {
                tileMap[i, basicPosZ].tag = "Area";
            }
            break;
        #endregion

        #region グラビトンブレイク
        case 9:
            GameDirector.Instance.IsMultiEffect = true;
            break;
        #endregion

        #region コメットブロー
        case 12:
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
        #endregion

        #region アストラルリベリオン
        case 20:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 1 && basicPosZ > 1)
                tileMap[basicPosX-2, basicPosZ-2].tag = "Area"; //←↑←↑
            if (basicPosZ > 0 && basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosX > 0 && basicPosZ > 1)
                tileMap[basicPosX-1, basicPosZ-2].tag = "Area"; //↑↑←
            if (basicPosX < 9 && basicPosZ > 1)
                tileMap[basicPosX+1, basicPosZ-2].tag = "Area"; //↑↑→
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 8 && basicPosZ > 1)
                tileMap[basicPosX+2, basicPosZ-2].tag = "Area"; //→↑→↑
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 8)
                tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            if (basicPosX < 8 && basicPosZ > 0)
                tileMap[basicPosX+2, basicPosZ-1].tag = "Area"; //→→↑
            if (basicPosX < 8 && basicPosZ < 9)
                tileMap[basicPosX+2, basicPosZ+1].tag = "Area"; //→→↓
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 8 && basicPosZ < 8)
                tileMap[basicPosX+2, basicPosZ+2].tag = "Area"; //→↓→↓
            if (basicPosX < 9 && basicPosZ < 9)
                tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 8)
                tileMap[basicPosX, basicPosZ+2].tag = "Area"; //↓↓
            if (basicPosX < 9 && basicPosZ < 8)
                tileMap[basicPosX+1, basicPosZ+2].tag = "Area"; //↓↓→
            if (basicPosX > 0 && basicPosZ < 8)
                tileMap[basicPosX-1, basicPosZ+2].tag = "Area"; //↓↓←
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 1 && basicPosZ < 8)
                tileMap[basicPosX-2, basicPosZ+2].tag = "Area"; //←↓←↓
            if (basicPosX > 0 && basicPosZ < 9)
                tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 1 && basicPosZ < 9)
                tileMap[basicPosX-2, basicPosZ+1].tag = "Area"; //←←↓
            if (basicPosX > 1 && basicPosZ > 0)
                tileMap[basicPosX-2, basicPosZ-1].tag = "Area"; //←←↑
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;
        #endregion

        #region グラビトンオフセッツ
        case 23:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 8)
                tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            break;
        #endregion

        #region 知性の光
        case 25:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 1 && basicPosZ > 1)
                tileMap[basicPosX-2, basicPosZ-2].tag = "Area"; //←↑←↑
            if (basicPosX > 0 && basicPosZ > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosX < 9 && basicPosZ < 9)
                tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            break;
        #endregion

        #region 願いの代償
        case 26:
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region ドラゴニックブレス
        case 29:
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
        #endregion

        #region タイダルウェーブ
        case 30:
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
        #endregion

        #region シルフ・サイクロン
        case 31:
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
        #endregion

        #region テラリウムグレイブ
        case 32:
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
        #endregion

        #region 次元断裂
        case 34:
            if (basicPosX > 1 && basicPosZ > 1)
                tileMap[basicPosX-2, basicPosZ-2].tag = "Area"; //←↑←↑
            if (basicPosZ > 0 && basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 1)
                tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosX > 0 && basicPosZ > 1)
                tileMap[basicPosX-1, basicPosZ-2].tag = "Area"; //↑↑←
            if (basicPosX < 9 && basicPosZ > 1)
                tileMap[basicPosX+1, basicPosZ-2].tag = "Area"; //↑↑→
            if (basicPosZ > 0)
                tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 8 && basicPosZ > 1)
                tileMap[basicPosX+2, basicPosZ-2].tag = "Area"; //→↑→↑
            if (basicPosX < 9 && basicPosZ > 0)
                tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 8)
                tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            if (basicPosX < 8 && basicPosZ > 0)
                tileMap[basicPosX+2, basicPosZ-1].tag = "Area"; //→→↑
            if (basicPosX < 8 && basicPosZ < 9)
                tileMap[basicPosX+2, basicPosZ+1].tag = "Area"; //→→↓
            if (basicPosX < 9)
                tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 8 && basicPosZ < 8)
                tileMap[basicPosX+2, basicPosZ+2].tag = "Area"; //→↓→↓
            if (basicPosX < 9 && basicPosZ < 9)
                tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 8)
                tileMap[basicPosX, basicPosZ+2].tag = "Area"; //↓↓
            if (basicPosX < 9 && basicPosZ < 8)
                tileMap[basicPosX+1, basicPosZ+2].tag = "Area"; //↓↓→
            if (basicPosX > 0 && basicPosZ < 8)
                tileMap[basicPosX-1, basicPosZ+2].tag = "Area"; //↓↓←
            if (basicPosZ < 9)
                tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 1 && basicPosZ < 8)
                tileMap[basicPosX-2, basicPosZ+2].tag = "Area"; //←↓←↓
            if (basicPosX > 0 && basicPosZ < 9)
                tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 1)
                tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 1 && basicPosZ < 9)
                tileMap[basicPosX-2, basicPosZ+1].tag = "Area"; //←←↓
            if (basicPosX > 1 && basicPosZ > 0)
                tileMap[basicPosX-2, basicPosZ-1].tag = "Area"; //←←↑
            if (basicPosX > 0)
                tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;
        #endregion

        default:
            break;
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
                if ((GameDirector.Instance.IsBasePointInArea == true && tileMap[x, z].tag == "Search") || tileMap[x, z].tag == "Area")
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