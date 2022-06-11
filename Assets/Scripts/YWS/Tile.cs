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
        //アクティブフェイズでプレイヤーが行動可能な時、使用するカードが選択されている、かつ必要分のコストが選択されており、さらにこのマスがクリックされた場合
        if (GameDirector.Instance.CanPlayerControl == true && IsMouseOver == true && GameDirector.Instance.IsCardSelect == true && GameDirector.Instance.PayedCost == GameDirector.Instance.NeedCost && Input.GetMouseButtonDown(0))
        {
            //検索用のタグを付ける
            this.tag = "Selected";
            GameDirector.Instance.NeedSearch = true;
        }
    }

    /// <summary>
    /// マウスがカードの上に乗っている時
    /// </summary>
    private void OnMouseOver()
    {
        tile.color = new Color(1, 1, 1, 0.5f);
        IsMouseOver = true;
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    private void OnMouseExit()
    {
        tile.color = new Color(1, 1, 1, 0);
        IsMouseOver = false;
    }
}