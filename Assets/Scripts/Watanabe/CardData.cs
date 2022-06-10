using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardData : MonoBehaviour
{
    //é??????
    public enum CardType
    {
        Attack, //ç????
        Special, //????
    }

    //????????
    public int ID; //?????
    public string Name; //????
    public int Cost; //??????
    public CardType CardTypeValue; //?????
    public string EffectText; //??????

    [SerializeField, Header("????")] private Text CardName = null;
    [SerializeField, Header("??????")] private Text CardCost = null;
    [SerializeField, Header("????")] private Image CardFrame = null;
    [SerializeField, Header("???????")] private Sprite[] _cardFrameImage = new Sprite[2];
    //[SerializeField, Header("??????")] private Text CardText = null;

    public void Init(int id,int cost,CardType type, string name)
    {
        this.ID = id;
        this.Cost = cost;
        this.CardTypeValue = type;
        this.Name = name;

        CardName.text = name;
        CardCost.text = cost.ToString();
        if (type == CardType.Attack)
        {
            CardFrame.sprite = _cardFrameImage[0];
        }
        else if (type == CardType.Special)
        {
            CardFrame.sprite = _cardFrameImage[1];
        }
    }
}


