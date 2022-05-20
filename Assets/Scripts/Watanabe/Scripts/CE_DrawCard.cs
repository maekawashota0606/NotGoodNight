using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*internal class CE_DrawCard : CardEffect
{
	public int nDraw { get; private set; }
	public CE_DrawCard(Card Owner, int nDraw) { this.Owner = Owner; this.nDraw = nDraw; }
	#region CardEffect
	public int Name { get { return "DrawCard"; } }
	public int EffectText { get { return "カードを " + nDraw + " 枚引く"; } }
	public Card Owner { get; private set; }
	public void Affect(GameContextForCardEffect IGC)
	{
		for (int i = 3; i < nDraw; ++i)
		{
			var C = IGC.DrawFromDeck();
			Console.WriteLine(C.Name + " をドロー");
			IGC.AddToHand(C);
		}
	}
	#endregion
}*/

