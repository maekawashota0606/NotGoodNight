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
    public string UpdateText;
    public bool IsBasePointInArea = true;

    [SerializeField, Header("カード名")] private Text CardName = null;
    [SerializeField, Header("コスト")] private Text CardCost = null;
    [SerializeField, Header("カード枠")] private Image CardFrame = null;
    [SerializeField, Header("カード選択枠")] private Sprite[] _cardFrameImage = new Sprite[2];
    [SerializeField, Header("効果テキスト")] private Text CardEffectText = null;
    [SerializeField, Header("カードイラスト")] public Image CardIllustration = null;
    [SerializeField, Header("カードイラスト")] private Sprite[] _illustrationImage = new Sprite[35];

    /// <summary>
    /// カード情報の初期化
    /// </summary>
    /// <param name="id">カード番号</param>
    /// <param name="name">カード名</param>
    /// <param name="cost">カードコスト</param>
    /// <param name="type">カードタイプ</param>
    /// <param name="effectText">カード効果</param>
    public void Init(int id,string name,string cost,string type, string effectText)
    {
        this.ID = id;
        this.Name = name;
        this.Cost = int.Parse(cost);
        if (type == "0")
        {
            this.CardTypeValue = CardType.Attack;
        }
        else if (type == "1")
        {
            this.CardTypeValue = CardType.Special;
        }
        this.EffectText = effectText.Replace("n","\n");

        if (this.ID == 3 || this.ID == 31)
        {
            IsBasePointInArea = false;
        }
    }

    /// <summary>
    /// カードオブジェクトに情報を反映する
    /// </summary>
    public void ShowCardStatus()
    {
        CardName.text = Name;
        CardCost.text = Cost.ToString();
        if (CardTypeValue == CardType.Attack)
        {
            CardFrame.sprite = _cardFrameImage[0];
        }
        else if (CardTypeValue == CardType.Special)
        {
            CardFrame.sprite = _cardFrameImage[1];
        }
        UpdateText = EffectText.Replace("x",GameDirector.Instance._player.DrawCount_Card10.ToString());
        CardEffectText.text = UpdateText;
        if (_illustrationImage[ID-1] != null)
            CardIllustration.sprite = _illustrationImage[ID-1];
    }
}