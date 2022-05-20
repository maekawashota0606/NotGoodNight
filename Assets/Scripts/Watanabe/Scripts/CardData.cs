using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    //íÞ
    public enum CardType
    {
        Attack,     //Uijóô¶j
        Defence    //häiìô¶j
    }
    //øÊ
    public enum CardEffective
    {
        Kill,          //jó
        Draw,       //h[
        Move,       //Ú®
        Stop,        //âØ
        Dump,      //ÌÄé
        Create,     //¶¬
        Heel,        //ñ 
        Trump,     //ØèD
        Border,     //{[_[
        Up,          // ø«ã°iè¦ÎðãÉ °éj
        Copy,       //Rs[
        Reduse,   //RXgy¸
        Extra,      //O±Â
    }

    public int ID = 0;
    public int Cost = 1;
    public CardType CardTypeValue = CardData.CardType.Attack;
    public CardEffective CardEffectiveValue = CardData.CardEffective.Kill;

    public void Init(int id,int cost,CardType type,CardEffective effective)
    {
        this.ID = id;
        this.Cost = cost;
        this.CardTypeValue = type;
        this.CardEffectiveValue = effective;
    }
}
