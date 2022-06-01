using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardClass : MonoBehaviour
{
    public List<CardData> CardList = new List<CardData>();
    //public CardData Card = new CardData();


    public CardData GetCardData(int id)
    {
        foreach(CardData cd in CardList)
        {
            if (id == cd.ID) return cd;
        }
        return null;
    }
}