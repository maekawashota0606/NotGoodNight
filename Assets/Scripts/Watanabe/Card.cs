using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : CardData
{
    void Start()
    {
        base.Init(1,1,CardData.CardType.Attack,"AAA");
    }
    void Update()
    {
        
    }
}