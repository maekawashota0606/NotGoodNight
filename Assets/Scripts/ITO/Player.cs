using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    #region カードごとの専用変数

    //効果の持続ターンカウント
    public int EffectTurn_Card14 = 0;

    #endregion

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

    /// <summary>
    /// カードドロー
    /// </summary>
    public void DrawCard()
    {
        int ID = Random.Range(1,36);
        //int ID = 13;
        //アクティブフェイズでのプレイヤーの行動としてのドロー
        if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.CanPlayerControl == true)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,0,CardData.CardType.Special,ID.ToString());
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
            newCard.Init(ID,0,CardData.CardType.Special,ID.ToString());
            hands.Add(newCard);
        }
    }

    /// <summary>
    /// 使用カードとコストカードの削除
    /// </summary>
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
        GameDirector.Instance.IsMultiEffect = false;
    }

    /// <summary>
    /// 特殊カードの効果処理
    /// </summary>
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
                GameDirector.Instance.IsSpecialCardEffect = true;
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

    /// <summary>
    /// 二個以上の効果を持つカードの効果処理
    /// </summary>
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

        case 13: //複製魔法
            CopyCard_Card13(GameDirector.Instance.CopyNum);
            break;

        default:
            break;
        }
        IsEffect = false;
    }

    /// <summary>
    /// 持続系の効果の効果終了チェック
    /// </summary>
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

    public void CopyCard_Card13(int cardID)
    {
        GameObject genCard = Instantiate(cardPrefab, playerHand);
        Card newCard = genCard.GetComponent<Card>();
        newCard.Init(cardID,0,CardData.CardType.Special,cardID.ToString());
        hands.Add(newCard);
        GameDirector.Instance.CopyNum = 0;
        GameDirector.Instance.IsSpecialCardEffect = false;
    }
}
