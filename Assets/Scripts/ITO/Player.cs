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
    public List<GameObject> hands = new List<GameObject>();
    //スコア
    public int Score = 0;
    //スコア表示テキスト
    [SerializeField] private Text scoreText = null;
    //ライフ
    public int Life = 3;
    //ライフ表示テキスト
    [SerializeField] private Text lifeText = null;
    public bool IsEffect = false;

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
            hands.Add(genCard);
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
            hands.Add(genCard);
        }
    }

    public void DeleteUsedCard()
    {
        //使用されたカードすべてにタグがついているので、それらを手札から見つけ出して削除する
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].tag == "Selected")
            {
                Destroy(hands[i]);
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
        if (GameDirector.Instance.CanPlayerControl == true && GameDirector.Instance.IsCardSelect == true && GameDirector.Instance.PayedCost == GameDirector.Instance.NeedCost)
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

            default:
                break;
            }
            IsEffect = false;
            GameDirector.Instance.IsCardUsed = true;
            GameDirector.Instance.CanPlayerControl = false;
            GameDirector.Instance.IsPlayerSelectMove = true;
        }
    }
}
