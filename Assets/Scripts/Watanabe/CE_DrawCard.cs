//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using namespace ContosoData;


//internal class CardEffective : CardEffect
//{
//    public int nDraw { get; private set; }
//    public CardEffective(Card Owner, int nDraw) { this.Owner = Owner; this.nDraw = nDraw; }
//    #region CardEffect
//    public int Name { get { return "DrawCard"; } }
//    public int EffectText { get { return "カードを " + nDraw + " 枚引く"; } }
//    public Card Owner { get; private set; }
//    public void Affect(GameContextForCardEffect IGC)
//    {
//        for (int i = 0; i < nDraw; ++i)
//        {
//            var C = IGC.DrawFromDeck();
//            Console.WriteLine(C.Name + " をドロー");
//            IGC.AddToHand(C);
//        }
//    }
//    #endregion
//}

