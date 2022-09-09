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
    //｜番号｜名前｜コスト｜カードタイプ｜破壊効果持ちかどうか｜効果テキスト｜
    public List<string[]> _cardData = new List<string[]>();

    [SerializeField, Header("カードプレハブ")] private GameObject cardPrefab = null;
    [SerializeField, Header("手札置き場")] public Transform playerHand = null;
    //手札
    public List<Card> hands = new List<Card>();
    //スコア
    public static int Score = 0;
    [SerializeField, Header("スコアテキスト")] private Text scoreText = null;
    //ライフ
    public int Life = 3;
    [SerializeField, Header("盤面")] private Image Board = null;
    [SerializeField, Header("盤面の画像")] private Sprite[] BoardImage = new Sprite[3];
    [SerializeField, Header("選択カードの置き場")] private Image SelectedCardSpace = null;
    [SerializeField, Header("選択カード置き場の画像")] private Sprite[] SelectedCardSpaceImage = new Sprite[2];
    //ボタンのダブルクリック防止用フラグ
    private bool IsClick = false;
    //隕石の引き寄せを行う時に使うデータのリスト
    public List<Meteorite> MoveList = new List<Meteorite>();
    public List<int> targetPosXList = new List<int>();
    public List<int> targetPosZList = new List<int>();

    #region カードごとの専用変数

    //効果によるドローが発生するかどうか
    public bool IsDrawEffect = false;
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

        //獲得スコアを随時更新で画面に表示させる
        scoreText.text = " " + Score.ToString("d6");
        if (Life > 0 && Life <= 3)
        {
            Board.sprite = BoardImage[Life-1];
        }
        else if (Life > 3)
        {
            Board.sprite = BoardImage[2];
        }

        if (GameDirector.Instance.SelectedCard == null || GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost)
        {
            SelectedCardSpace.sprite = SelectedCardSpaceImage[0];
        }
        else if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost)
        {
            SelectedCardSpace.sprite = SelectedCardSpaceImage[1];
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
        int totalCardNum = 0;
        if (GameDirector.Instance.SelectedCard != null)
        {
            totalCardNum++;
        }
        if (GameDirector.Instance.costCardList.Count > 0)
        {
            totalCardNum += GameDirector.Instance.costCardList.Count;
        }
        totalCardNum += hands.Count;
        //手札は10枚が上限なので、10枚の状態でドローは行えない
        if (totalCardNum == 10 || IsClick == true)
        {
            return;
        }

        int[] CardID = new int[32]{1,2,3,4,5,6,7,8,9,10,11,12,14,15,16,18,19,20,21,22,23,24,25,26,27,29,30,31,32,33,34,35};
        //int ID = Random.Range(1,36);
        //int ID = 6;
        int DrawNum = Random.Range(0,CardID.Length);
        int ID = CardID[DrawNum];
        SoundManager.Instance.PlaySE(7);
        //ゲーム開始時の初期手札のドロー
        if (GameDirector.Instance.gameState == GameDirector.GameState.standby)
        {
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4],_cardData[ID][5]);
            //カードオブジェクトをリストに入れる
            hands.Add(newCard);
        }
        //アクティブフェイズでのプレイヤーの行動としてのドロー
        else if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            IsClick = true;
            GameObject genCard = Instantiate(cardPrefab, playerHand);
            Card newCard = genCard.GetComponent<Card>();
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4],_cardData[ID][5]);
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
            newCard.Init(ID,_cardData[ID][1],_cardData[ID][2],_cardData[ID][3],_cardData[ID][4],_cardData[ID][5]);
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
        GameDirector.Instance.ResetCardPosition();
    }

    /// <summary>
    /// 使用コストカードの削除
    /// </summary>
    public void DeleteUsedCost()
    {
        //使用されたコストカードすべてを削除する
        for (int i = 0; i < GameDirector.Instance.costCardList.Count; i++)
        {
            Destroy(GameDirector.Instance.costCardList[i].gameObject);
            GameDirector.Instance.costCardList.RemoveAt(i);
            i--;
        }
    }

    /// <summary>
    /// 使用カードの削除
    /// </summary>
    public void DeleteUsedCard()
    {
        //使用されたカードを削除する
        Destroy(GameDirector.Instance.SelectedCard.gameObject);
        GameDirector.Instance.PayedCost = 0;
    }

    /// <summary>
    /// 持続系の効果の効果終了チェック
    /// </summary>
    public void CheckEffectTurn()
    {
        #region グラビトンリジェクト
        if (EffectTurn_Card14 > 0)
        {
            EffectTurn_Card14--;
            if (EffectTurn_Card14 == 0)
            {
                GameDirector.Instance.DoMeteorFall = true;
                GameDirector.Instance.CanMeteorGenerate = true;
            }
        }
        #endregion

        #region 魔力障壁
        if (EffectTurn_Card19 > 0)
        {
            EffectTurn_Card19--;
            if (EffectTurn_Card19 == 0)
            {
                GameDirector.Instance.DoMeteorFall = true;
                GameDirector.Instance.CanMeteorGenerate = true;
            }
        }
        #endregion
    }

    /// <summary>
    /// 複製魔法用関数
    /// </summary>
    /// <param name="cardID"></param>
    public void CopyCard_Card13(int cardID)
    {
        GameObject genCard = Instantiate(cardPrefab, playerHand);
        Card newCard = genCard.GetComponent<Card>();
        newCard.Init(cardID,_cardData[cardID][1],_cardData[cardID][2],_cardData[cardID][3],_cardData[cardID][4],_cardData[cardID][5]);
        hands.Add(newCard);
        GameDirector.Instance.CopyNum_Card13 = 0;
        GameDirector.Instance.WaitCopy_Card13 = false;
        GameDirector.Instance.IsPlayerSelectMove = true;
    }

    public void CheckIsMoveFinish()
    {
        int FinishedNum = 0;
        if (GameDirector.Instance.WaitingMove == true && MoveList.Count != 0)
        {
            for (int num = 0; num < MoveList.Count; num++)
            {
                if (MoveList[num].MoveFinished == true)
                {
                    FinishedNum++;
                }
                else
                {
                    break;
                }
            }

            if (FinishedNum == MoveList.Count)
            {
                GameDirector.Instance.IsPlayerSelectMove = true;
                Map.Instance.UpdateMapData();
            }
        }
    }
}