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
    public int EffectTurn_Card19 = 0;
    public int EffectTurn_Card28 = 0;

    #endregion

    /// <summary>
    /// csvファイスのセットアップ
    /// </summary>
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

        //残りライフによって盤面の画像を随時更新させる
        if (Life > 0 && Life <= 3)
        {
            Board.sprite = BoardImage[Life-1];
        }
        else if (Life > 3)
        {
            Board.sprite = BoardImage[2];
        }

        //選択された使用カードが使用可能かどうかによって使用カード置き場の画像を切り替える
        if (GameDirector.Instance.SelectedCard == null || GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost)
        {
            SelectedCardSpace.sprite = SelectedCardSpaceImage[0];
        }
        else if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost)
        {
            SelectedCardSpace.sprite = SelectedCardSpaceImage[1];
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
        //使用カードと使用コストは手札から外されているため、手札の数を計算する
        int totalCardNum = 0;
        if (IsDrawEffect == false)
        {
            if (GameDirector.Instance.SelectedCard != null)
            {
                totalCardNum++;
            }
            if (GameDirector.Instance.costCardList.Count > 0)
            {
                totalCardNum += GameDirector.Instance.costCardList.Count;
            }
        }
        totalCardNum += hands.Count;
        //手札は10枚が上限なので、10枚の状態でドローは行えない
        if (totalCardNum == 10)
        {
            return;
        }

        //ドローするカードの番号を乱数で生成する
        int ID = Random.Range(1,36);
        //int ID = 9;

        SoundManager.Instance.PlaySE(7);
        GameObject genCard = Instantiate(cardPrefab, playerHand);
        Card newCard = genCard.GetComponent<Card>();
        newCard.Init(ID);
        //効果によるドロー
        if (GameDirector.Instance.gameState == GameDirector.GameState.effect && IsDrawEffect == true)
        {
            if (GameDirector.Instance.SelectedCard.ID == 18 && newCard.Cost > 0)
            {
                newCard.Cost--;
            }
            else if (GameDirector.Instance.SelectedCard.ID == 22)
            {
                newCard.Cost = 0;
            }
        }
        //カードオブジェクトをリストに入れる
        hands.Add(newCard);
        //手札のカードの位置を調整する
        GameDirector.Instance.ResetCardPosition();
        IsClick = false;
    }

    /// <summary>
    /// アクティブフェイズでのプレイヤーの行動としてのドロー
    /// </summary>
    public void DrawButtonOnClick()
    {
        //使用カードと使用コストは手札から外されているため、手札の数を計算する
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
        if (GameDirector.Instance.gameState != GameDirector.GameState.active || IsClick == true || totalCardNum == 10)
        {
            return;
        }

        IsClick = true;
        if (totalCardNum < 5)
        {
            for (int i = 0; i < 5 - totalCardNum; i++)
            {
                DrawCard();
            }
        }
        else
        {
            DrawCard();
        }
        DrawCount_Card10++;
        //カードを引いたら隕石落下フェイズに移行する
        GameDirector.Instance.gameState = GameDirector.GameState.fall;
    }

    /// <summary>
    /// 使用コストカードの削除
    /// </summary>
    public void DeleteUsedCost()
    {
        //コストカードが一枚もない場合は削除を行わなくていい
        if (GameDirector.Instance.costCardList.Count == 0)
        {
            return;
        }
        //コストリストにあるカードをすべて削除する
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
        //使用カードが存在しない場合は削除を行わなくていい
        if (GameDirector.Instance.SelectedCard == null)
        {
            return;
        }
        //使用カードとして登録しているカードを削除する
        Destroy(GameDirector.Instance.SelectedCard.gameObject);
        GameDirector.Instance.SelectedCard = null;
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

        #region 光の奔流
        if (EffectTurn_Card28 > 0)
        {
            if (EffectTurn_Card28 < 4)
            {
                for (int i = 0; i < 2; i++)
                {
                    DrawCard();
                }
            }
            EffectTurn_Card28--;
        }
        #endregion
    }

    /// <summary>
    /// 隕石の引き寄せ効果を使用する時、移動が完了しているかどうかを調べる関数
    /// </summary>
    public void CheckIsMoveFinish()
    {
        int FinishedNum = 0;
        if (GameDirector.Instance.WaitingMove == true && MoveList.Count != 0)
        {
            //順番に隕石を調べる
            for (int num = 0; num < MoveList.Count; num++)
            {
                //隕石の移動が完了したら、カウンターを増やす
                if (MoveList[num].MoveFinished == true)
                {
                    FinishedNum++;
                }
                else
                {
                    break;
                }
            }

            //すべての隕石の移動が完了した場合
            if (FinishedNum == MoveList.Count)
            {
                GameDirector.Instance.IsPlayerSelectMove = true;
                //マップの更新
                Map.Instance.UpdateMapData();
            }
        }
    }
}