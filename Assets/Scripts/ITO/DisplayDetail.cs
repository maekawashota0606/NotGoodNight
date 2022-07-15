using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDetail : MonoBehaviour
{
    [SerializeField, Header("コスト")] private Text costText = null;
    [SerializeField, Header("カード名")] private Text nameText = null;
    [SerializeField, Header("効果テキスト")] private Text effectText = null;
    [SerializeField, Header("カードイラスト")] private Image displayIllustration = null;
    [SerializeField, Header("イラスト配列")] private Sprite[] Illustration = new Sprite[35];

    void Update()
    {
        if (GameDirector.Instance.WatchingCard != null)
        {
            costText.text = GameDirector.Instance.WatchingCard.Cost.ToString();
            nameText.text = GameDirector.Instance.WatchingCard.Name;
            effectText.text = GameDirector.Instance.WatchingCard.EffectText.Replace("x",GameDirector.Instance._player.DrawCount_Card10.ToString());
            if(Illustration[GameDirector.Instance.WatchingCard.ID - 1] != null)
            {
                displayIllustration.sprite = Illustration[GameDirector.Instance.WatchingCard.ID - 1];
            }
        }
        if (GameDirector.Instance.WatchingCard == null)
        {
            costText.text = "";
            nameText.text = "";
            effectText.text = "";
            displayIllustration.sprite = Illustration[35];
        }
    }
}
