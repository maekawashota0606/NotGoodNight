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
        // 値を初期化
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

        //アクティブフェイズでプレイヤーが行動可能な時、使用するカードが選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.CanPlayerControl == true && IsMouseOver == true && GameDirector.Instance.IsCardSelect == true && GameDirector.Instance.PayedCost == GameDirector.Instance.NeedCost && Input.GetMouseButtonDown(0))
        {
            GameDirector.Instance.NeedSearch = true;
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
        GameDirector.Instance.IsTileNeedSearch = true;
    }

    /// <summary>
    /// マウスがマスの上から離れた時
    /// </summary>
    private void OnMouseExit()
    {
        tile.color = new Color(1, 1, 1, 0);
        this.tag = "Untagged";
        IsMouseOver = false;
        GameDirector.Instance.IsTileNeedSearch = false;
        GameDirector.Instance.IsMouseLeaveTile = true;
    }
}