using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Header("点滅させるオブジェクト")] private SpriteRenderer tile = null;
    //マウスが乗っているかどうか
    private bool IsMouseOver = false;
    private bool IsSEPlayed = false;
    
    private void Start()
    {
        //色の値を初期化
        tile.color = new Color(255,255,255,0);
    }

    void Update()
    {
        //このマスがカード範囲内に含まれている場合、光らせる
        if (this.tag == "Area")
        {
            if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.SelectedCard.ID != 9)
            {
                if (GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost)
                {
                    tile.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    tile.color = new Color(1,0,0,0.5f);
                }
            }
        }
        else if (this.tag == "Untagged")
        {
            tile.color = new Color(1, 1, 1, 0);
        }

        //効果処理フェイズで収束カードが使用カードとして選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.gameState == GameDirector.GameState.active || GameDirector.Instance.gameState == GameDirector.GameState.extra)
        {
            ConfirmeArea();
        }
    }

    private void ConfirmeArea()
    {
        if (GameDirector.Instance.SelectedCard == null)
        {
            return;
        }

        //効果処理フェイズで収束カードが使用カードとして選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.SelectedCard.CardTypeValue == CardData.CardType.Convergence && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlaySE(5);
            bool IsTarget = false;
            //破壊効果を含むカードを選択している場合、この範囲内で破壊できる隕石が存在するかどうかを調べる
            if (GameDirector.Instance.SelectedCard.IsDestroyEffect == true)
            {
                for (int z = 0; z < 10; z++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (TileMap.Instance.tileMap[x, z].tag == "Search" || TileMap.Instance.tileMap[x, z].tag == "Area")
                        {
                            Vector3 checkPos = new Vector3(x, 0, -z);
                            //一つでも隕石が存在する場合、探索を終了させる
                            if (!Map.Instance.CheckEmpty(checkPos))
                            {
                                IsTarget = true;
                            }
                        }
                    }
                }
                //隕石が一つも存在していない場合、ここで処理を終了させる
                if (IsTarget == false)
                {
                    return;
                }
            }
            //範囲内に隕石が存在するか、破壊効果を含まない効果だった場合、カード効果処理フェイズに移行する
            GameDirector.Instance.gameState = GameDirector.GameState.effect;
        }
    }

    /// <summary>
    /// マウスがマスの上に乗っている時
    /// </summary>
    private void OnMouseOver()
    {
        if (IsSEPlayed == false)
        {
            SoundManager.Instance.PlaySE(4);
            IsSEPlayed = true;
        }

        //必要分のコストが支払われている場合、白く表示する
        if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost)
        {
            tile.color = new Color(1, 1, 1, 0.5f);
        }
        //必要分のコストが支払われていない場合、赤く表示する
        else if (GameDirector.Instance.SelectedCard == null || GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost)
        {
            tile.color = new Color(1,0,0,0.5f);
        }
        //このマスを特定するためのタグ付け
        this.tag = "Search";
        IsMouseOver = true;
        GameDirector.Instance.IsMouseOnTile = true;
    }

    /// <summary>
    /// マウスがマスの上から離れた時
    /// </summary>
    private void OnMouseExit()
    {
        IsSEPlayed = false;
        tile.color = new Color(1, 1, 1, 0);
        this.tag = "Untagged";
        IsMouseOver = false;
        GameDirector.Instance.IsMouseOnTile = false;
        //マウスが他のマスに移動した場合、一回全てのマスのタグを初期化する
        TileMap.Instance.ResetTileTag();
    }
}