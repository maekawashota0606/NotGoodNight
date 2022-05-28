using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    //種類
    public enum CardType
    {
        Attack,     //攻撃（破壊呪文）
        Defence    //防御（援護呪文）
    }
    //効果
    public enum CardEffective
    {
        Destroy,    //破壊
        Draw,       //ドロー
        Move,       //移動
        Stop,        //停滞
        Dump,      //捨てる
        Create,     //生成
        Heel,        //回復 
        Trump,     //切り札
        Border,     //ボーダー
        Up,          // 引き上げ（隕石を上にあげる）
        Copy,       //コピー
        Reduse,   //コスト軽減
        Extra,      //外部干渉
    }
    public int ID = 0;
    public int Cost = 1;
    public CardType CardTypeValue = CardData.CardType.Attack;
    public CardEffective CardEffectiveValue = CardData.CardEffective.Destroy;

    public void Init(int id,int cost,CardType type,CardEffective effective)
    {
        this.ID = id;
        this.Cost = cost;
        this.CardTypeValue = type;
        this.CardEffectiveValue = effective;
    }
}


