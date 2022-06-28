using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CardData : MonoBehaviour
{
    //カードタイプ
    public enum CardType
    {
        Attack, //攻撃
        Special, //特殊
    }

    //カードステータス
    public int ID; //カード番号
    public string Name; //カード名
    public int Cost; //カードコスト
    public CardType CardTypeValue; //カードタイプ
    public string EffectText; //効果テキスト

    [SerializeField, Header("カード名")] private Text CardName = null;
    [SerializeField, Header("コスト")] private Text CardCost = null;
    [SerializeField, Header("カード枠")] private Image CardFrame = null;
    [SerializeField, Header("カード選択枠")] private Sprite[] _cardFrameImage = new Sprite[2];
    //[SerializeField, Header("効果テキスト")] private Text CardText = null;

    public void Init(int id,string name,string cost,string type, string effectText)
    {
        this.ID = id;
        this.Name = name;
        this.Cost = int.Parse(cost);
        if (type == "0")
        {
            this.CardTypeValue = CardType.Attack;
            CardFrame.sprite = _cardFrameImage[0];
        }
        else if (type == "1")
        {
            this.CardTypeValue = CardType.Special;
            CardFrame.sprite = _cardFrameImage[1];
        }
        this.EffectText = effectText;

        CardName.text = name;
        CardCost.text = cost;
    }
}


