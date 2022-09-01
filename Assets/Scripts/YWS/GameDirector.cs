using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Random = UnityEngine.Random;
using DG.Tweening;

public class GameDirector : SingletonMonoBehaviour<GameDirector>
{
    [SerializeField, Header("基本獲得スコア")] public int point = 0;
    [SerializeField, Header("隕石生成オブジェクト")] MeteorGenerator _generator = null;
    [SerializeField, Header("画面を振動させるオブジェクト")] ShakeByRandom _shaker = null;
    [SerializeField] public Player _player = null;
    
    // 隕石用リストを宣言
    public List<Meteorite> meteors = new List<Meteorite>();
    //ゲームの進行状況
    public GameState gameState = GameState.none;
    //プレイヤーが行動を選択したかどうか
    public bool IsPlayerSelectMove = false;
    //隕石を生成するかどうか
    public bool CanMeteorGenerate = true;
    //ターンチェンジで生成する隕石の数
    private int MeteorGenNum = 2;
    //ターンカウント
    private int TurnCount = 1;
    //隕石の落下を行うかどうか
    public bool DoMeteorFall = true;
    //勝敗判定用フラグ
    public bool IsPlayerWin = false;

    #region カード使用関連の変数
    [SerializeField, Header("拡散カード使用ボタン")] private GameObject CardUseButton = null;
    [SerializeField, Header("選択カード置き場")] private Transform SelectedCardPosition = null;
    [SerializeField, Header("選択コスト置き場")] private Transform SelectedCostPosition = null;
    public List<Card> costCardList = new List<Card>();
    //カーソルが合っているカードオブジェクト
    public Card WatchingCard = null;
    //使用カードとして選択されているカードオブジェクト
    public Card SelectedCard = null;
    //盤面にマウスカーソルが乗っているか
    public bool IsMouseOnTile = false;
    //使用カードに複数の効果が存在しているかどうか
    public bool IsMultiEffect = false;
    //カードの使用が確定したかどうか
    public bool IsCardUsingConfirm = false;
    //隕石が破壊されたのかどうか
    public bool IsMeteorDestroyed = false;
    //このターンで破壊された隕石の数
    public int DestroyedNum = 0;
    //すでに選択されているコストの数
    public int PayedCost = 0;
    //複製魔法用フラグ
    public bool WaitCopy_Card13 = false;
    //複製魔法用のコピー元の番号
    public int CopyNum_Card13 = 0;
    public bool WaitingMove = false;

    #endregion
    
    public enum GameState
    {
        none,
        standby,
        active,
        effect,
        fall,
        judge,
        end,
        ended,
    }

    void Start()
    {
        SoundManager.Instance.PlayBGM(1);
        //色々初期化
        Init();
        _player.SetCsv();
    }

    void Update()
    {
        //Debug.Log(gameState);
        //Debug.Log(TurnCount);
        switch (gameState)
        {
            case GameState.standby: //スタンバイフェイズ
                //手札が5枚になるまでドロー
                for (int hand = 0; hand < 5; hand++)
                {
                    _player.DrawCard();
                }

                //盤面の上4列に隕石を生成
                //横一列につき、隕石を二個ランダムに生成
                for (int i = 3; i > -1; i--)
                {
                    MeteorSet(2,i);
                }
                //ドローと隕石の処理が終了したら、アクティブフェイズに移行する
                gameState = GameState.active;
                break;

            case GameState.active: //アクティブフェイズ
                if(SelectedCard != null)
                {
                    if (SelectedCard.CardTypeValue == CardData.CardType.Convergence)
                    {
                        TileMap.Instance.FindBasePoint();
                    }
                    else if (SelectedCard.CardTypeValue == CardData.CardType.Diffusion && PayedCost >= SelectedCard.Cost)
                    {
                        CardUseButton.SetActive(true);
                    }
                }

                //ここらへんα版用
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _player.Score += 10000;
                }
                break;

            case GameState.effect: //カード効果処理フェイズ
                if (IsPlayerSelectMove == true)
                {
                    //使用されたカードをすべて削除する
                    _player.DeleteUsedCost();
                    _player.DeleteUsedCard();
                    SelectedCard = null;
                    TileMap.Instance.ResetTileTag();
                    IsMeteorDestroyed = false;
                    //効果処理が終了したら、隕石落下フェイズに移行する
                    gameState = GameState.fall;
                }

                if (SelectedCard != null)
                {
                    if (WaitingMove == false)
                    {
                        SelectedCard.CardEffect();
                        if (WaitCopy_Card13 == false)
                        {
                            IsPlayerSelectMove = true;
                        }
                    }
                    else
                    {
                        _player.CheckIsMoveFinish();
                    }
                }
                break;

            case GameState.fall: //隕石落下フェイズ
                IsPlayerSelectMove = false;
                WaitingMove = false;
                if (DoMeteorFall == true)
                {
                    meteors = meteors.OrderBy(meteor => meteor.transform.position.z).ThenBy(meteors => meteors.transform.position.x).ToList();
                    for (int num = 0; num < meteors.Count; num++)
                    {
                        var x = (int)meteors[num].transform.position.x;
                        var z = (int)meteors[num].transform.position.z * -1;
                        //隕石の下１マスが空白だった場合
                        if (z < 9)
                        {
                            //隕石オブジェクトを１マス下に移動
                            meteors[num].StartFall();
                        }
                        //隕石の下１マスが空白ではなかった場合
                        //下から順に処理を行っているため、隕石の下に他の隕石が存在する事はありえない
                        else if (z == 9 && meteors[num].FallFinished == false)
                        {
                            //隕石オブジェクトを削除する
                            Destroy(meteors[num].gameObject);
                            //リストから削除
                            meteors.RemoveAt(num);
                            //マップから削除
                            Map.Instance.map[z, x] = Map.Instance.empty;
                            //プレイヤーのライフを減らす
                            _player.Life--;
                            _shaker.StartShake(1f,20f,20f);
                            num--;
                        }
                    }
                }
                if (meteors.Count == 0 || meteors[meteors.Count-1].FallFinished == true || DoMeteorFall == false)
                {
                    gameState = GameState.judge;
                }
                break;

            case GameState.judge: //ゲーム終了判定
                //プレイヤーのライフが0以下の場合、ゲームを終了させる
                if (_player.Life <= 0)
                {
                    gameState = GameState.end;
                    break;
                }
                //プレイヤーのスコアが100000以上の場合
                else if (_player.Score >= 100000)
                {
                    //かつマップ上に隕石が存在しない場合、ゲームを終了させる
                    if (Map.Instance.CheckMap())
                    {
                        gameState = GameState.end;
                        break;
                    }
                    //マップ上に隕石が存在する場合、隕石の生成を終了させる
                    else
                    {
                        CanMeteorGenerate = false;
                    }
                }
                //以上の条件すべてに当てはまらない場合
                else
                {
                    //新しい隕石が生成可能の場合、新たに隕石を生成する
                    {
                        //盤面の最上列に隕石を生成
                        //隕石はランダムに生成
                        if (CanMeteorGenerate == true)
                        {
                            MeteorSet(MeteorGenNum,0);
                        }
                    }
                    //１０ターンごとに生成する隕石の数を１個増やす（上限は６個）
                    if (TurnCount % 10 == 0 && MeteorGenNum < 6)
                    {
                        MeteorGenNum++;
                    }
                }
                
                //ターンカウントを１つ増やす
                TurnCount++;
                Map.Instance.CheckMapData();
                IsCardUsingConfirm = false;
                if (DestroyedNum > 0)
                {
                    AddScore();
                    DestroyedNum = 0;
                }
                //持続系のカード効果のターンカウントを進める、効果が切れたら効果の処理を元に戻す
                _player.CheckEffectTurn();
                //アクティブフェイズに戻る
                gameState = GameState.active;
                break;

            case GameState.end: //ゲーム終了処理
                if (_player.Life == 0)
                {
                    IsPlayerWin = false;
                    Debug.Log("ゲームオーバー");
                }
                else
                {
                    IsPlayerWin = true;
                    Debug.Log("ゲームクリア");
                }
                SoundManager.Instance.StopBGM();
                gameState = GameState.ended;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Init()
    {
        CardUseButton.SetActive(false);
        gameState = GameState.standby;
        IsPlayerSelectMove = false;
        CanMeteorGenerate = true;
        MeteorGenNum = 2;
        TurnCount = 1;
        DoMeteorFall = true;
        IsPlayerWin = false;
        SelectedCard = null;
        IsMouseOnTile = false;
        IsMultiEffect = false;
        IsCardUsingConfirm = false;
        IsMeteorDestroyed = false;
        DestroyedNum = 0;
        PayedCost = 0;
        WaitCopy_Card13 = false;
        _player.hands = new List<Card>();
    }

    /// <summary>
    /// 指定した行にランダムに指定した数だけ隕石を生成
    /// </summary>
    /// <param name="amount">生成する数</param>
    /// <param name="columns">生成する列</param>
    public void MeteorSet(int amount, int columns)
    {
        //生成した隕石の数
        int created = 0; 

        while (true)
        {
            //生成する場所を乱数で出す
            int x = Random.Range(0,10);
            Vector3 checkPos = new Vector3(x, 0, -columns);
            if (Map.Instance.CheckEmpty(checkPos))
            {
                _generator.Generate(checkPos);
                created++;
                Map.Instance.map[columns, x] = Map.Instance.meteor;
            }

            if (created == amount)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 指定した座標に隕石を生成
    /// </summary>
    /// <param name="targerPosX">指定されたx座標</param>
    /// <param name="targetPosZ">指定されたz座標</param>
    public void MeteorSetTarget(int targerPosX, int targetPosZ)
    {
        Vector3 TargetPos = new Vector3(targerPosX, 0, -targetPosZ);
        _generator.Generate(TargetPos);
        Map.Instance.map[targetPosZ, targerPosX] = Map.Instance.meteor;
    }

    /// <summary>
    /// スコアの加算
    /// </summary>
    public void AddScore()
    {
        var GetScore = 1000 * GameDirector.Instance.DestroyedNum * (1 + GameDirector.Instance.DestroyedNum * 0.1f);
        _player.Score += (int)GetScore;
    }

    public void SetSelectCard(Card card)
    {
        if (card.ID == 11 || card.ID == 15 || card.ID == 19 || (card.ID == 35 && _player.hands.Count != 1))
        {
            return;
        }
        
        _player.hands.Remove(card);
        SelectedCard = card;
        //効果が処理された後に削除するために、タグを付けておく
        card.tag = "Selected";
        card.transform.SetParent(SelectedCardPosition);
        card.transform.DOMove(SelectedCardPosition.transform.position, 0.1f, true);
        ResetCardPosition();
    }

    public void SetCostCard(Card card)
    {
        _player.hands.Remove(card);
        costCardList.Add(card);
        //選択されているコストの数を加算する
        switch(card.ID)
        {
        case 11: //サクリファイス・レプリカ
            PayedCost += 2;
            break;

        default:
            PayedCost++;
            break;
        }
        //コストとして使用された時に削除する用にタグを付けておく
        card.tag = "Cost";
        card.transform.SetParent(SelectedCostPosition);
        Vector3 movePoint = new Vector3(0, 160 - 45 * (costCardList.Count - 1), 0);
        card.transform.DOLocalMove(movePoint, 0.1f, true);
        card.GetComponentInChildren<Canvas>().sortingOrder = costCardList.Count - 1;
        ResetCardPosition();
    }

    public void ResetToHand(Card card, bool IsCost)
    {
        if (IsCost)
        {
            costCardList.Remove(card);
        }
        _player.hands.Add(card);
        card.tag = "Untagged";
        card.transform.SetParent(_player.playerHand);
        Vector3 movePoint = new Vector3(-180 + 45 * (_player.hands.Count - 1), 0, 0);
        card.transform.DOLocalMove(movePoint, 0.1f, true);
        card.GetComponentInChildren<Canvas>().sortingOrder = _player.hands.Count - 1;
        ResetCostPosition();
        CardUseButton.SetActive(false);
    }

    public void ResetCardPosition()
    {
        for (int num = 0; num < _player.hands.Count; num++)
        {
            _player.hands[num].GetComponentInChildren<Canvas>().sortingOrder = num;
            _player.hands[num].transform.localPosition = new Vector3(-180 + 45 * num, 0, 0);
        }
    }

    public void ResetCardPositionWhenWatching()
    {
        bool StartReset = false;
        for (int num = 0; num < _player.hands.Count; num++)
        {
            if (_player.hands[num].tag == "Watching" && StartReset == false)
            {
                _player.hands[num].transform.localPosition = new Vector3(-180 + 45 * num + 8, 0, 0);
                if (num != _player.hands.Count)
                {
                    StartReset = true;
                }
            }
            else if (StartReset == true)
            {
                _player.hands[num].transform.localPosition = new Vector3(-180 + 45 * num + 126, 0, 0);
            }
        }
    }

    public void ResetCostPosition()
    {
        for (int num = 0; num < costCardList.Count; num++)
        {
            costCardList[num].GetComponentInChildren<Canvas>().sortingOrder = num;
            costCardList[num].transform.localPosition = new Vector3(0, 160 - 45 * num, 0);
        }
    }

    public void ResetCostPositionWhenWatching()
    {
        bool StartReset = false;
        for (int num = 0; num < costCardList.Count; num++)
        {
            if (costCardList[num].tag == "Watching" && StartReset == false)
            {
                costCardList[num].transform.localPosition = new Vector3(0, 160 - 45 * num - 11, 0);
                if (num != costCardList.Count)
                {
                    StartReset = true;
                }
            }
            else if (StartReset == true)
            {
                costCardList[num].transform.localPosition = new Vector3(0, 160 - 45 * num - 199, 0);
            }
        }
    }

    public void UseButtonOnClick()
    {
        gameState = GameState.effect;
        CardUseButton.SetActive(false);
    }
}