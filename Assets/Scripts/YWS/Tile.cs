using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Header("点滅させるオブジェクト")] private SpriteRenderer tile = null;
    //マウスが乗っているかどうか
    private bool IsMouseOver = false;
    
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
            tile.color = new Color(1, 1, 1, 0.5f);
        }
        else if (this.tag == "Untagged")
        {
            tile.color = new Color(1, 1, 1, 0);
        }

        //効果処理フェイズで使用するカードが選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.gameState == GameDirector.GameState.effect && GameDirector.Instance.SelectedCard != null && GameDirector.Instance.SelectedCard.CardTypeValue == CardData.CardType.Attack && GameDirector.Instance.IsCardUsingConfirm == true && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            //範囲が選択された場合、範囲内の隕石を検索する
            TileMap.Instance.MeteorDestory();
        }
    }

    /// <summary>
    /// マウスがマスの上に乗っている時
    /// </summary>
    private void OnMouseOver()
    {
        if (GameDirector.Instance.IsBasePointInArea == true)
        {
            tile.color = new Color(1, 1, 1, 0.5f);
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
        tile.color = new Color(1, 1, 1, 0);
        this.tag = "Untagged";
        IsMouseOver = false;
        GameDirector.Instance.IsMouseOnTile = false;
        //マウスが他のマスに移動した場合、一回全てのマスのタグを初期化する
        TileMap.Instance.ResetTileTag();
    }
}