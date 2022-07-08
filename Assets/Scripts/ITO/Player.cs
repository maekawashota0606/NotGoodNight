using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    //csvファイル用変数
    public TextAsset _csvFile;
    //並び順
    //｜番号｜名前｜コスト｜カードタイプ｜効果テキスト｜
    public List<string[]> _cardData = new List<string[]>();

    //カードプレハブ
    [SerializeField] private GameObject cardPrefab = null;
    //手札置き場
    [SerializeField] private Transform playerHand = null;
    //手札
    public static List<Card> hands = new List<Card>();
    //スコア
    public int Score = 0;
    //スコア表示テキスト
    [SerializeField] private Text scoreText = null;
    //ライフ
    public int Life = 3;
    //ライフ表示テキスト
    [SerializeField] private Text lifeText = null;

    #region カードごとの専用変数

    //効果の持続ターンカウント
    public int EffectTurn_Card14 = 0;

    #endregion

    public void SetCsv()
    {
        StringReader reader = new StringReader(_csvFile.text);
        
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            _cardData.Add(line.Split(','));
        }
    }

    private void Update()
    {
        //ライフがマイナスに行かないようにする
        if (Life <= 0)
        {
            Life = 0;
        }

        //獲得スコアと現在ライフを随時更新で画面に表示させる
        scoreText.text = "Score / " + Score.ToString("d6");
        lifeText.text = "Life / " + Life.ToString();

        if (GameDirector.Instance.WaitCopy_Card13 == true && GameDirector.Instance.CopyNum_Card13 != 0)
        {
            CopyCard_Card13(GameDirector.Instance.CopyNum_Card13);
        }
    }

    /// <summary>
    /// カードドロー
    /// </summary>
    public void DrawCard()
    {
        //手札は10枚が上限なので、10枚の状態でドローは行えない
        if (hands.Count == 10)
        {
            return;
        }

        int ID = Random.Range(1,36);
        //ゲーム開始時の初期手札のドロー
        if (GameDirector.Instance.gameState == GameDirector.GameState.standby)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4]);
            //カードオブジェクトをリストに入れる
            hands.Add(newCard);
        }
        //アクティブフェイズでのプレイヤーの行動としてのドロー
        else if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4]);
            //カードオブジェクトをリストに入れる
            hands.Add(newCard);
            //カードを引いたら隕石落下フェイズに移行する
            GameDirector.Instance.gameState = GameDirector.GameState.fall;
        }
        //効果によるドロー
        else if (GameDirector.Instance.gameState == GameDirector.GameState.effect)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4]);
            if (GameDirector.Instance.SelectedCardObject.ID == 18 && newCard.Cost > 0)
            {
                newCard.Cost--;
            }
            else if (GameDirector.Instance.SelectedCardObject.ID == 22)
            {
                newCard.Cost = 0;
            }
            //カードオブジェクトをリストに入れる
            hands.Add(newCard);
        }
    }

    public void DeleteUsedCost()
    {
        //使用されたカードすべてにタグがついているので、それらを手札から見つけ出して削除する
        for (int i = 0; i < hands.Count; i++)
        {
            if (hands[i].tag == "Cost")
            {
                Destroy(hands[i].gameObject);
                hands.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// 使用カードの削除
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
        GameDirector.Instance.PayedCost = 0;

        if (GameDirector.Instance.WaitCopy_Card13 == true && GameDirector.Instance.CopyNum_Card13 != 0)
        {
            CopyCard_Card13(GameDirector.Instance.CopyNum_Card13);
        }
    }

    /// <summary>
    /// 特殊カードの効果処理
    /// </summary>
    public void SpecialCardEffect()
    {
        //if (GameDirector.Instance.SelectedCardObject != null)
        //{
            switch(GameDirector.Instance.SelectedCardObject.ID)
            {
            case 5: //アストラルリコール
                for (int i = 0; i < 3; i++)
                {
                    DrawCard();
                }
                break;

            case 13: //複製魔法
                GameDirector.Instance.WaitCopy_Card13 = true;
                break;

            case 14: //グラビトンリジェクト
                EffectTurn_Card14 = 3;
                GameDirector.Instance.DoMeteorFall = false;
                GameDirector.Instance.CanMeteorGenerate = false;
                break;

            case 16: //不破の城塞
                Debug.Log("hands num " + hands.Count);
                for (int i = 0; i < hands.Count; i++)
                {
                    Debug.Log("Total " + GameDirector.Instance.meteors.Count);
                    int DestoryNum = Random.Range(0,GameDirector.Instance.meteors.Count);
                    Debug.Log("Hit " + DestoryNum);
                    Debug.Log("Destroy " + GameDirector.Instance.meteors[DestoryNum]);
                    //隕石オブジェクトを削除する
                    Destroy(GameDirector.Instance.meteors[DestoryNum]);
                    //リストから削除
                    GameDirector.Instance.meteors.RemoveAt(DestoryNum);
                    //マップから削除
                    Map.Instance.map[(int)GameDirector.Instance.meteors[DestoryNum].transform.position.z*-1, (int)GameDirector.Instance.meteors[DestoryNum].transform.position.x] = Map.Instance.empty;
                }
                break;

            case 18: //詮索するはばたき
                for (int i = 0; i < 3; i++)
                {
                    DrawCard();
                }
                break;

            case 21: //残光のアストラル
                int DrawNum = 2;
                DrawNum += Score / 30000 * 2;
                for (int i = 0; i < DrawNum; i++)
                {
                    DrawCard();
                }
                break;

            case 22: //至高天の顕現
                for (int i = 0; i < 2; i++)
                {
                    DrawCard();
                }
                break;

            case 27: //復興の灯
                Life++;
                break;

            case 35: //ラスト・ショット
                //手札がこのカード一枚じゃないとこのカードは使えない
                /*if (hands.Count != 1)
                {
                    return;
                }
                else
                {*/
                    for (int i = 0; i < 7; i++)
                    {
                        DrawCard();
                    }
                //}
                break;

            default:
                break;
            }
        //}
    }

    /// <summary>
    /// 二個以上の効果を持つカードの効果処理
    /// </summary>
    public void ExtraEffect()
    {
        switch(GameDirector.Instance.SelectedCardObject.ID)
        {
        case 12: //コメットブロー
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;

        default:
            break;
        }
        GameDirector.Instance.IsMultiEffect = false;
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

        if (GameDirector.Instance.IsEffect_Card19)
        {
            GameDirector.Instance.DoMeteorFall = true;
            GameDirector.Instance.IsEffect_Card19 = false;
        }
    }

    public void CopyCard_Card13(int cardID)
    {
        GameObject genCard = Instantiate(cardPrefab, playerHand);
        Card newCard = genCard.GetComponent<Card>();
        newCard.Init(cardID,_cardData[cardID][1],_cardData[cardID][2],_cardData[cardID][3],_cardData[cardID][4]);
        hands.Add(newCard);
        GameDirector.Instance.CopyNum_Card13 = 0;
        GameDirector.Instance.WaitCopy_Card13 = false;
        GameDirector.Instance.IsPlayerSelectMove = true;
    }
}