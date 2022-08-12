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
    private List<BoxCollider2D> handsColliders = new List<BoxCollider2D>();
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
    private List<Meteorite> MoveList = new List<Meteorite>();

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

        //int[] CardID = new int[28]{1,2,3,4,5,8,10,11,12,13,14,15,16,18,19,20,21,22,23,24,25,27,29,30,31,32,33,35};
        //int ID = Random.Range(1,36);
        int ID = 7;
        //int DrawNum = Random.Range(0,CardID.Length);
        //int ID = CardID[DrawNum];
        SoundManager.Instance.PlaySE(7);
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

    private void TuneCardCollider()
    {
        for (int num = 0; num < hands.Count; num++)
        {
            handsColliders[num] = hands[num].GetComponent<BoxCollider2D>();
            Debug.Log(num + handsColliders[num].size.x);
        }
    }

    /// <summary>
    /// 使用コストカードの削除
    /// </summary>
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
        #region アストラルリコール
        case 5:
            IsDrawEffect = true;
            for (int i = 0; i < 4; i++)
            {
                DrawCard();
            }
            break;
        #endregion
        
        #region 星磁力
        case 6: //
            break;
        #endregion

        #region グラビトンコア
        case 7:
            MoveList = new List<Meteorite>();
            List<int> targetPosXList = new List<int>();
            List<int> targetPosZList = new List<int>();
            int LockedNum = 0;
            for (int x = 0; x < 10; x++)
            {
                for (int z = 0; z < 10; z++)
                {
                    if (TileMap.Instance.tileMap[x, z].tag == "Search" || TileMap.Instance.tileMap[x, z].tag == "Area")
                    {
                        Vector3 checkPos = GameDirector.Instance._DEFAULT_POSITION + new Vector3(x, 0, -z);
                        if (!Map.Instance.CheckEmpty(checkPos))
                        {
                            TileMap.Instance.tileMap[x,z].tag = "Lock";
                            LockedNum++;
                        }
                        else
                        {
                            targetPosXList.Add(x);
                            targetPosZList.Add(z);
                        }
                    }
                }
            }

            for (int num = 0; num < targetPosXList.Count; num++)
            {
                if (GameDirector.Instance.meteors.Count == 0 || MoveList.Count == 9 || GameDirector.Instance.meteors.Count == MoveList.Count + LockedNum)
                {
                    break;
                }

                int chosenNum = Random.Range(0,GameDirector.Instance.meteors.Count);
                int checkx = (int)GameDirector.Instance.meteors[chosenNum].transform.position.x;
                int checkz = -(int)GameDirector.Instance.meteors[chosenNum].transform.position.z;
                if (TileMap.Instance.tileMap[checkx, checkz].tag != "Lock" && TileMap.Instance.tileMap[checkx, checkz].tag != "Move")
                {
                    TileMap.Instance.tileMap[checkx,checkz].tag = "Move";
                    MoveList.Add(GameDirector.Instance.meteors[chosenNum]);
                }
                else
                {
                    num--;
                }
            }
            GameDirector.Instance.WaitingMove = true;
            for (int num = 0; num < MoveList.Count; num++)
            {
                MoveList[num].MoveToTargetPoint(targetPosXList[num], -targetPosZList[num]);
            }
            break;
        #endregion
        
        #region 星屑収集
        case 10:
            for (int i = 0; i < DrawCount_Card10; i++)
            {
                if (GameDirector.Instance.meteors.Count == 0)
                {
                    break;
                }
                SoundManager.Instance.PlaySE(6);
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
        #endregion

        #region 複製魔法
        case 13:
            if (hands.Count > 1)
            {
                GameDirector.Instance.WaitCopy_Card13 = true;
            }
            break;
        #endregion

        #region グラビトンリジェクト
        case 14:
            EffectTurn_Card14 = 3;
            GameDirector.Instance.DoMeteorFall = false;
            GameDirector.Instance.CanMeteorGenerate = false;
            break;
        #endregion

        #region 不破の城塞
        case 16:
            for (int i = 0; i < hands.Count; i++)
            {
                if (GameDirector.Instance.meteors.Count == 0 || hands.Count == 0)
                {
                    break;
                }
                SoundManager.Instance.PlaySE(6);
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
        #endregion

        #region 詮索するはばたき
        case 18:
            IsDrawEffect = true;
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region 残光のアストラル
        case 21:
            IsDrawEffect = true;
            int DrawNum = 2;
            DrawNum += Score / 30000 * 2;
            for (int i = 0; i < DrawNum; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region 至高天の顕現
        case 22:
            IsDrawEffect = true;
            for (int i = 0; i < 2; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region 隕石の儀式
        case 24:
            IsDrawEffect = true;
            for (int x = 0; x < 10; x++)
            {
                Vector3 checkPos = GameDirector.Instance._DEFAULT_POSITION + new Vector3(x, 0, 0);
                if (Map.Instance.CheckEmpty(checkPos))
                {
                    GameDirector.Instance.MeteorSetTarget(x,0);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region 願いの代償
        case 26:
            IsDrawEffect = true;
            for (int z = 0; z < 10; z++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (TileMap.Instance.tileMap[x, z].tag == "Search" || TileMap.Instance.tileMap[x, z].tag == "Area")
                    {
                        Vector3 checkPos = GameDirector.Instance._DEFAULT_POSITION + new Vector3(x, 0, -z);
                        if (Map.Instance.CheckEmpty(checkPos))
                        {
                            GameDirector.Instance.MeteorSetTarget(x,z);
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region 復興の灯
        case 27:
            Life++;
            break;
        #endregion

        #region 流星群
        case 33:
            for (int i = 0; i < 70; i++)
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
        #endregion

        #region ラスト・ショット
        case 35:
            IsDrawEffect = true;
            for (int i = 0; i < 9; i++)
            {
                DrawCard();
            }
            break;
        #endregion

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
        #region グラビトンブレイク
        case 9:
            for (int i = 0; i < TileMap.Instance.checkListX.Count; i++)
            {
                Vector3 basicPos = new Vector3(TileMap.Instance.checkListX[i], 0, -TileMap.Instance.checkListZ[i]);
                Vector3 UpPos = basicPos + Vector3.forward;
                Vector3 DownPos = basicPos + Vector3.back;
                Vector3 LeftPos = basicPos + Vector3.left;
                Vector3 RightPos = basicPos + Vector3.right;

                if (-(int)UpPos.z > -1)
                {
                    if (!Map.Instance.CheckEmpty(UpPos))
                    {
                        TileMap.Instance.checkListX.Add((int)UpPos.x);
                        TileMap.Instance.checkListZ.Add(-(int)UpPos.z);
                    }
                }
                if (-(int)DownPos.z < 10)
                {
                    if (!Map.Instance.CheckEmpty(DownPos))
                    {
                        TileMap.Instance.checkListX.Add((int)DownPos.x);
                        TileMap.Instance.checkListZ.Add(-(int)DownPos.z);
                    }
                }
                if ((int)LeftPos.x > -1)
                {
                    if (!Map.Instance.CheckEmpty(LeftPos))
                    {
                        TileMap.Instance.checkListX.Add((int)LeftPos.x);
                        TileMap.Instance.checkListZ.Add(-(int)LeftPos.z);
                    }
                }
                if ((int)RightPos.x < 10)
                {
                    if (!Map.Instance.CheckEmpty(RightPos))
                    {
                        TileMap.Instance.checkListX.Add((int)RightPos.x);
                        TileMap.Instance.checkListZ.Add(-(int)RightPos.z);
                    }
                }

                for (int num = 0; num < GameDirector.Instance.meteors.Count; num++)
                {
                    if (GameDirector.Instance.meteors[num].transform.position.x == (int)basicPos.x && GameDirector.Instance.meteors[num].transform.position.z == (int)basicPos.z)
                    {
                        //マップから削除
                        Map.Instance.map[(int)GameDirector.Instance.meteors[num].transform.position.z*-1, (int)GameDirector.Instance.meteors[num].transform.position.x] = Map.Instance.empty;
                        //隕石オブジェクトを削除する
                        Destroy(GameDirector.Instance.meteors[num].gameObject);
                        //リストから削除
                        GameDirector.Instance.meteors.RemoveAt(num);
                        GameDirector.Instance.DestroyedNum++;
                    }
                }
            }
            TileMap.Instance.checkListX = new List<int>();
            TileMap.Instance.checkListZ = new List<int>();
            break;
        #endregion

        #region コメットブロー
        case 12:
            IsDrawEffect = true;
            for (int i = 0; i < 5; i++)
            {
                DrawCard();
            }
            break;
        #endregion

        #region アストラルリベリオン
        case 20:
            IsDrawEffect = true;
            if (Score >= 50000)
            {
                for (int i = 0; i < 7; i++)
                {
                    DrawCard();
                }
            }
            break;
        #endregion

        #region グラビトンオフセッツ
        case 23:
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
                    Vector3 checkPos = GameDirector.Instance._DEFAULT_POSITION + new Vector3(x, 0, -z);
                    if (Map.Instance.CheckEmpty(checkPos))
                    {
                        GameDirector.Instance.MeteorSetTarget(x,z);
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
        #endregion

        #region 知性の光
        case 25:
            IsDrawEffect = true;
            for (int i = 0; i < GameDirector.Instance.DestroyedNum; i++)
            {
                DrawCard();
            }
            break;
        #endregion

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
        newCard.Init(cardID,_cardData[cardID][1],_cardData[cardID][2],_cardData[cardID][3],_cardData[cardID][4]);
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