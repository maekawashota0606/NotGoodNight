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

    [SerializeField, Header("カードプレハブ")] private GameObject cardPrefab = null;
    [SerializeField, Header("手札置き場")] private Transform playerHand = null;
    //手札
    public static List<Card> hands = new List<Card>();
    //スコア
    public int Score = 0;
    [SerializeField, Header("スコアテキスト")] private Text scoreText = null;
    //ライフ
    public int Life = 3;
    [SerializeField, Header("ライフテキスト")] private Text lifeText = null;
    [SerializeField, Header("盤面")] private Image Board = null;
    [SerializeField, Header("盤面の画像")] private Sprite[] BoardImage = new Sprite[3];
    //ボタンのダブルクリック防止用フラグ
    private bool IsClick = false;

    #region カードごとの専用変数

    //効果によるドローが発生するかどうか
    private bool IsDrawEffect = false;
    //星屑収集用のこのゲーム中、何回ドローボタンでカードをドローしたかを保存する変数
    public int DrawCount_Card10 = 0;
    //効果の持続ターンカウント
    public int EffectTurn_Card14 = 0;
    public static int EffectTurn_Card19 = 0;

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
        if (Life > 0 && Life <= 3)
        {
            Board.sprite = BoardImage[Life-1];
        }
        else if (Life > 3)
        {
            Board.sprite = BoardImage[2];
        }

        if (GameDirector.Instance.WaitCopy_Card13 == true && GameDirector.Instance.CopyNum_Card13 != 0)
        {
            CopyCard_Card13(GameDirector.Instance.CopyNum_Card13);
        }

        if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            IsClick = false;
        }
    }

    /// <summary>
    /// カードドロー
    /// </summary>
    public void DrawCard()
    {
        //手札は10枚が上限なので、10枚の状態でドローは行えない
        if (hands.Count == 10 || IsClick == true)
        {
            return;
        }

        //int[] CardID = new int[25]{1,2,3,4,5,8,10,11,12,13,14,15,16,18,19,20,21,22,25,27,29,30,31,32,35};
        int ID = Random.Range(1,36);
        //int DrawNum = Random.Range(0,CardID.Length);
        //int ID = CardID[DrawNum];
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
            IsClick = true;
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4]);
            //カードオブジェクトをリストに入れる
            hands.Add(newCard);
            DrawCount_Card10++;
            //カードを引いたら隕石落下フェイズに移行する
            GameDirector.Instance.gameState = GameDirector.GameState.fall;
        }
        //効果によるドロー
        else if (GameDirector.Instance.gameState == GameDirector.GameState.effect && IsDrawEffect == true)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4]);
            if (GameDirector.Instance.SelectedCard.ID == 18 && newCard.Cost > 0)
            {
                newCard.Cost--;
            }
            else if (GameDirector.Instance.SelectedCard.ID == 22)
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
    }

    /// <summary>
    /// 特殊カードの効果処理
    /// </summary>
    public void SpecialCardEffect()
    {
        switch(GameDirector.Instance.SelectedCard.ID)
        {
        case 5: //アストラルリコール
            IsDrawEffect = true;
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;
        
        case 10: //星屑収集
            for (int i = 0; i < DrawCount_Card10; i++)
            {
                if (GameDirector.Instance.meteors.Count == 0)
                {
                    break;
                }
                int DestoryNum = Random.Range(0,GameDirector.Instance.meteors.Count);
                //マップから削除
                Map.Instance.map[(int)GameDirector.Instance.meteors[DestoryNum].transform.position.z*-1, (int)GameDirector.Instance.meteors[DestoryNum].transform.position.x] = Map.Instance.empty;
                //隕石オブジェクトを削除する
                Destroy(GameDirector.Instance.meteors[DestoryNum].gameObject);
                //リストから削除
                GameDirector.Instance.meteors.RemoveAt(DestoryNum);
                GameDirector.Instance.DestroyedNum++;
            }
            GameDirector.Instance.AddScore();
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
            for (int i = 0; i < hands.Count; i++)
            {
                if (GameDirector.Instance.meteors.Count == 0)
                {
                    break;
                }
                int DestoryNum = Random.Range(0,GameDirector.Instance.meteors.Count);
                //マップから削除
                Map.Instance.map[(int)GameDirector.Instance.meteors[DestoryNum].transform.position.z*-1, (int)GameDirector.Instance.meteors[DestoryNum].transform.position.x] = Map.Instance.empty;
                //隕石オブジェクトを削除する
                Destroy(GameDirector.Instance.meteors[DestoryNum].gameObject);
                //リストから削除
                GameDirector.Instance.meteors.RemoveAt(DestoryNum);
                GameDirector.Instance.DestroyedNum++;
            }
            break;

        case 18: //詮索するはばたき
            IsDrawEffect = true;
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;

        case 21: //残光のアストラル
            IsDrawEffect = true;
            int DrawNum = 2;
            DrawNum += Score / 30000 * 2;
            for (int i = 0; i < DrawNum; i++)
            {
                DrawCard();
            }
            break;

        case 22: //至高天の顕現
            IsDrawEffect = true;
            for (int i = 0; i < 2; i++)
            {
                DrawCard();
            }
            break;

        case 27: //復興の灯
            Life++;
            break;

        case 33: //流星群
            for (int i = 0; i < 50; i++)
            {
                int ranX = Random.Range(0,10);
                int ranZ = Random.Range(0,10);
                if (TileMap.Instance.tileMap[ranX,ranZ].tag != "Area")
                {
                    TileMap.Instance.tileMap[ranX,ranZ].tag = "Area";
                }
                else
                {
                    i--;
                }
            }
            TileMap.Instance.MeteorDestory();
            break;

        case 35: //ラスト・ショット
            IsDrawEffect = true;
            for (int i = 0; i < 7; i++)
            {
                DrawCard();
            }
            break;

        default:
            break;
        }
        IsDrawEffect = false;
    }

    /// <summary>
    /// 二個以上の効果を持つカードの効果処理
    /// </summary>
    public void ExtraEffect()
    {
        switch(GameDirector.Instance.SelectedCard.ID)
        {
        case 12: //コメットブロー
            IsDrawEffect = true;
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;

        case 20: //アストラルリベリオン
            IsDrawEffect = true;
            if (Score >= 50000)
            {
                for (int i = 0; i < 5; i++)
                {
                    DrawCard();
                }
            }
            break;

        case 23: //グラビトンオフセッツ
            List<int> columnList = new List<int>{0,1,2,3,4};
            for (int num = 0; num < 4; num++)
            {
                if (columnList.Count == 0)
                {
                    break;
                }
                int z = Random.Range(0,columnList.Count);
                for (int x = 0; x < 10; x++)
                {
                    Vector3 checkPos = GameDirector.Instance._DEFAULT_POSITION + new Vector3(x, 0 ,-z);
                    if (Map.Instance.CheckEmpty(checkPos))
                    {
                        GameDirector.Instance.MeteorSet(1,z);
                        break;
                    }
                    else if (x == 9 && !Map.Instance.CheckEmpty(checkPos))
                    {
                        columnList.RemoveAt(z);
                        num--;
                    }
                }
            }
            break;

        case 25: //知性の光
            IsDrawEffect = true;
            for (int i = 0; i < GameDirector.Instance.DestroyedNum; i++)
            {
                DrawCard();
            }
            break;

        default:
            break;
        }
        IsDrawEffect = false;
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
            if (EffectTurn_Card14 == 0)
            {
                GameDirector.Instance.DoMeteorFall = true;
                GameDirector.Instance.CanMeteorGenerate = true;
            }
        }

        if (EffectTurn_Card19 > 0)
        {
            EffectTurn_Card19--;
            if (EffectTurn_Card19 == 0)
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
        newCard.Init(cardID,_cardData[cardID][1],_cardData[cardID][2],_cardData[cardID][3],_cardData[cardID][4]);
        hands.Add(newCard);
        GameDirector.Instance.CopyNum_Card13 = 0;
        GameDirector.Instance.WaitCopy_Card13 = false;
        GameDirector.Instance.IsPlayerSelectMove = true;
    }
}