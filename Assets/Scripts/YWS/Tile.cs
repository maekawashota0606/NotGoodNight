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
        //値を初期化
        tile.color = new Color(255,255,255,0);
    }

    void Update()
    {
        //このマスがカード範囲内に含まれている場合、光らせる
        if (this.tag == "Area")
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
        else if (this.tag == "Untagged")
        {
            tile.color = new Color(1, 1, 1, 0);
        }

        //効果処理フェイズで使用するカードが選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.SelectedCard != null && GameDirector.Instance.SelectedCard.CardTypeValue == CardData.CardType.Convergence && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            SoundManager.Instance.PlaySE(5);
            bool IsTarget = false;
            if (GameDirector.Instance.SelectedCard.IsDestroyEffect == true)
            {
                for (int z = 0; z < 10; z++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        if (TileMap.Instance.tileMap[x, z].tag == "Search" || TileMap.Instance.tileMap[x, z].tag == "Area")
                        {
                            Vector3 checkPos = new Vector3(x, 0, -z);
                            if (!Map.Instance.CheckEmpty(checkPos))
                            {
                                IsTarget = true;
                            }
                        }
                    }
                }

                if (IsTarget == false)
                {
                    return;
                }
            }
            //範囲が選択された場合、範囲内の隕石を検索する
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

        if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost)
        {
            tile.color = new Color(1, 1, 1, 0.5f);
        }
        else if (GameDirector.Instance.SelectedCard == null || GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost)
        {
            tile.color = new Color(1,0,0,0.5f);
        }
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