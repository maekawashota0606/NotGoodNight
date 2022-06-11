using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : CardData
{
    [SerializeField, Header("カード選択フレーム")] private Image image_component = null;
    //マウスがカードの上に乗っているかどうか
    public bool IsMouseOver = false;
    //クリックされた状態なのかどうか
    public bool IsClick = false;
    public bool IsCost = false;

    void Start()
    {
        base.Init(1,1,CardData.CardType.Attack,"AAA");
    }

    void Update()
    {
        if (GameDirector.Instance.PayedCost < GameDirector.Instance.NeedCost && GameDirector.Instance.IsCardSelect == true && IsClick == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            image_component.color = Color.green;
            IsClick = true;
            IsCost = true;
            GameDirector.Instance.PayedCost++;
            this.tag = "Selected";
        }
        //選択されていないカードをクリックしたら、そのカードの枠を赤色にする
        else if (GameDirector.Instance.IsCardSelect == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            image_component.color = Color.red;
            IsClick = true;
            GameDirector.Instance.IsCardSelect = true;
            GameDirector.Instance.NeedCost = this.Cost;
            if (this.Cost != 0)
            {
                GameDirector.Instance.NeedPayCost = true;
            }
            this.tag = "Selected";
        }

        //選択されたカードを右クリックしたら、そのカードの選択を解除し、枠を白色にする
        if (IsClick == true && IsMouseOver == true && Input.GetMouseButtonDown(1))
        {
            image_component.color = Color.white;
            IsClick = false;
            if (IsCost == true)
            {
                GameDirector.Instance.PayedCost--;
            }
            else
            {
                GameDirector.Instance.IsCardSelect = false;
                GameDirector.Instance.NeedCost = 0;
                GameDirector.Instance.NeedPayCost = false;
                GameDirector.Instance.PayedCost = 0;
                this.tag = "Untagged";
            }
        }

        if (IsCost == true && GameDirector.Instance.IsCardSelect == false)
        {
            image_component.color = Color.white;
            IsClick = false;
            IsCost = false;
        }

        //このカードが選択されていない場合
        if (IsClick == false)
        {
            //マウスが乗っていたら、カードの枠を黄色にする
            if (IsMouseOver == true)
            {
                image_component.color = Color.yellow;
            }
            //乗っていない場合、白色にする
            else
            {
                image_component.color = Color.white;
            }
        }
    }

    /// <summary>
    /// マウスがカードの上に乗っている時
    /// </summary>
    void OnMouseOver()
    {
        IsMouseOver = true;
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    void OnMouseExit()
    {
        IsMouseOver = false;
    }
}