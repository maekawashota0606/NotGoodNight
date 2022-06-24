using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //カードプレハブ
    [SerializeField] private GameObject cardPrefab = null;
    //手札置き場
    [SerializeField] private Transform playerHand = null;
    //手札
    public List<Card> hands = new List<Card>();
    //スコア
    public int Score = 0;
    //スコア表示テキスト
    [SerializeField] private Text scoreText = null;
    //ライフ
    public int Life = 3;
    //ライフ表示テキスト
    [SerializeField] private Text lifeText = null;
    public bool IsEffect = false;
    //効果の持続ターンカウント
    public int EffectTurn_Card14 = 0;

    private void Update()
    {
        //カード効果の処理が終了したら、使用カードとコストとして使われたカードを削除する
        if (GameDirector.Instance.IsCardUsed == true)
        {
            DeleteUsedCard();
        }

        //ライフがマイナスに行かないようにする
        if (Life <= 0)
        {
            Life = 0;
        }

        //獲得スコアと現在ライフを随時更新で画面に表示させる
        scoreText.text = "Score / " + Score.ToString("d6");
        lifeText.text = "Life / " + Life.ToString();
    }

    public void DrawCard()
    {
        //アクティブフェイズでのプレイヤーの行動としてのドロー
        if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.CanPlayerControl == true)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            hands.Add(newCard);
            if (IsEffect == false)
            {
                GameDirector.Instance.CanPlayerControl = false;
                GameDirector.Instance.IsPlayerSelectMove = true;
            }
        }
        //ゲーム開始時の初期手札のドロー
        else if (GameDirector.Instance.gameState == GameDirector.GameState.standby)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            hands.Add(newCard);
        }
    }

    public void DeleteUsedCard()
    {
        //使用されたカードすべてにタグがついているので、それらを手札から見つけ出して削除する
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].tag == "Selected")
            {
                Destroy(hands[i].gameObject);
                hands.RemoveAt(i);
                i--;
            }
        }
        GameDirector.Instance.IsCardUsed = false;
        GameDirector.Instance.IsCardSelect = false;
        GameDirector.Instance.NeedCost = 0;
        GameDirector.Instance.NeedPayCost = false;
        GameDirector.Instance.PayedCost = 0;
    }

    public void CardEffect()
    {
        if (GameDirector.Instance.CanPlayerControl == true && GameDirector.Instance.IsCardSelect == true && GameDirector.Instance.PayedCost >= GameDirector.Instance.NeedCost && GameDirector.Instance.IsAttackCard == false)
        {
            switch(GameDirector.Instance.SelectedCardNum)
            {
            case 5: //アストラルリコール
                for (int i = 0; i < 3; i++)
                {
                    IsEffect = true;
                    DrawCard();
                }
                break;

            case 13: //複製魔法

                break;

            case 14: //グラビトンリジェクト
                EffectTurn_Card14 = 3;
                GameDirector.Instance.DoMeteorFall = false;
                GameDirector.Instance.CanMeteorGenerate = false;
                break;

            default:
                break;
            }
            
            if (GameDirector.Instance.SelectedCardNum != 11 || GameDirector.Instance.SelectedCardNum == 15)
            {
                IsEffect = false;
                GameDirector.Instance.IsCardUsed = true;
                GameDirector.Instance.CanPlayerControl = false;
                GameDirector.Instance.IsPlayerSelectMove = true;
            }
        }
    }

    public void ExtraEffect()
    {
        switch(GameDirector.Instance.SelectedCardNum)
        {
        case 12: //コメットブロー
            for (int i = 0; i < 3; i++)
            {
                IsEffect = true;
                DrawCard();
            }
            break;

        default:
            break;
        }
        IsEffect = false;
    }

    public void CheckEffectTurn()
    {
        if (EffectTurn_Card14 > 0)
        {
            EffectTurn_Card14--;
            if (EffectTurn_Card14 < 0)
                EffectTurn_Card14 = 0;
            if (EffectTurn_Card14 == 0)
            {
                GameDirector.Instance.DoMeteorFall = true;
                GameDirector.Instance.CanMeteorGenerate = true;
            }
        }
    }
}
