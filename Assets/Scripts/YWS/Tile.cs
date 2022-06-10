using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Header("点滅させるオブジェクト")] private SpriteRenderer tile = null;
    private bool IsMouseOver = false;
    
    private void Start()
    {
        // 値を初期化
        tile.color = new Color(255,255,255,0);
    }

    void Update()
    {
        if (GameDirector.Instance.CanPlayerControl == true && IsMouseOver == true && GameDirector.Instance.IsCardSelect == true && Input.GetMouseButtonDown(0))
        {
            this.tag = "Selected";
            GameDirector.Instance.NeedSearch = true;
        }
    }

    private void OnMouseOver()
    {
        tile.color = new Color(1, 1, 1, 0.5f);
        IsMouseOver = true;
    }

    private void OnMouseExit()
    {
        tile.color = new Color(1, 1, 1, 0);
        IsMouseOver = false;
    }
}