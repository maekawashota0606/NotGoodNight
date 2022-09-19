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
    //このターンで破壊された隕石の数
    public int DestroyedNum = 0;
    //すでに選択されているコストの数
    public int PayedCost = 0;
    public bool WaitingMove = false;

    #endregion
    
    public enum GameState
    {
        none,
        standby,
        active,
        effect,
        extra,
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
                //使用カードが選択されている場合
                if(SelectedCard != null)
                {
                    //収束カードの場合、効果範囲を盤面上に表示する
                    if (SelectedCard.CardTypeValue == CardData.CardType.Convergence)
                    {
                        //プレイヤーがマウスカーソルで指している盤面のマスを特定し、カードの範囲を表示する
                        TileMap.Instance.FindBasePoint();
                    }
                    //拡散カードかつ必要分のコストを支払っている場合、カード使用ボタンを表示する
                    else if (SelectedCard.CardTypeValue == CardData.CardType.Diffusion && PayedCost >= SelectedCard.Cost)
                    {
                        CardUseButton.SetActive(true);
                    }
                }
                break;

            case GameState.effect: //カード効果処理フェイズ
                //カードの処理がすべて終了した場合、次のフェイズに移行する
                if (IsPlayerSelectMove == true)
                {
                    //使用されたカードをすべて削除する
                    _player.DeleteUsedCost();
                    _player.DeleteUsedCard();
                    //盤面のすべてマスのタグをリセットする
                    TileMap.Instance.ResetTileTag();
                    //隕石落下フェイズに移行する
                    gameState = GameState.fall;
                }

                //使用カードが選択されている場合
                if (SelectedCard != null)
                {
                    //かつ隕石の引き寄せによる移動中でない場合
                    if (WaitingMove == false)
                    {
                        //カードの効果を処理する
                        SelectedCard.CardEffect();
                        if (GameDirector.Instance.SelectedCard.ID != 13)
                        {
                            IsPlayerSelectMove = true;
                        }
                    }
                    else
                    {
                        //隕石引き寄せ効果の場合、全ての隕石が移動を終了しているかどうかをチェック
                        _player.CheckIsMoveFinish();
                    }
                }
                break;

            case GameState.extra: //複製魔法処理フェイズ
                CardUseButton.SetActive(false);
                //収束カードの場合、プレイヤーがマウスカーソルで指している盤面のマスを特定し、カードの範囲を表示する
                if (SelectedCard.CardTypeValue == CardData.CardType.Convergence)
                {
                    TileMap.Instance.FindBasePoint();
                }
                //拡散カードの場合、カード使用ボタンを表示する
                else if (SelectedCard.CardTypeValue == CardData.CardType.Diffusion)
                {
                    CardUseButton.SetActive(true);
                }
                break;

            case GameState.fall: //隕石落下フェイズ
                IsPlayerSelectMove = false;
                WaitingMove = false;

                //隕石の落下を止める効果がない場合にのみ、隕石の落下を行う
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
                            //盤面を振動させる
                            _shaker.StartShake(1f,20f,20f);
                            num--;
                        }
                    }
                }
                //隕石が存在しない、隕石の落下を行わない、またはすべての隕石の落下が終了した場合、ゲーム終了判定フェイズに移行する
                if (meteors.Count == 0 || CheckMeteorMove() == true || DoMeteorFall == false)
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
                else if (Player.Score >= 100000)
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
        DestroyedNum = 0;
        PayedCost = 0;
        _player.hands = new List<Card>();
        Player.Score = 0;
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
            //生成しようとしている座標が空白の場合にのみ生成を行う
            if (Map.Instance.CheckEmpty(checkPos))
            {
                _generator.Generate(checkPos);
                created++;
                Map.Instance.map[columns, x] = Map.Instance.meteor;
            }

            //指定された数の隕石が生成された場合、ループを終了させる
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

    public bool CheckMeteorMove()
    {
        int FinishedNum = 0;
        for (int num = 0; num < meteors.Count; num++)
        {
            if (meteors[num].FallFinished == true)
            {
                FinishedNum++;
            }
        }
        if (FinishedNum == meteors.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// スコアの加算
    /// </summary>
    public void AddScore()
    {
        var GetScore = 1000 * GameDirector.Instance.DestroyedNum * (1 + GameDirector.Instance.DestroyedNum * 0.1f);
        Player.Score += (int)GetScore;
    }

    /// <summary>
    /// 使用カードをセットする関数
    /// </summary>
    /// <param name="card">セットするカードオブジェクト</param>
    public void SetSelectCard(Card card)
    {
        card.transform.localScale = new Vector3(2.1f, 2.1f, 2.1f) * 1.1f;
        //手札リストから削除する
        _player.hands.Remove(card);
        //使用カードとして登録する
        SelectedCard = card;
        //使用カード置き場を親オブジェクトにする
        card.transform.SetParent(SelectedCardPosition);
        //カードをスライド移動させる
        card.transform.DOMove(SelectedCardPosition.transform.position, 0.1f, true);
        //残った手札の位置を調整
        ResetCardPosition();
    }

    /// <summary>
    /// コストカードをセットする関数
    /// </summary>
    /// <param name="card">セットするカードオブジェクト</param>
    public void SetCostCard(Card card)
    {
        //手札リストから削除する
        _player.hands.Remove(card);
        //使用コストとしてコストリストに登録する
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
        //使用コスト置き場を親オブジェクトにする
        card.transform.SetParent(SelectedCostPosition);
        Vector3 movePoint = new Vector3(0, 95 - 70 * (costCardList.Count - 1), 0);
        //カードをスライド移動させる
        card.transform.DOLocalMove(movePoint, 0.1f, true);
        //何枚目のコストかによってカードオブジェクトにあるキャンバスの表示順を変更する
        card.GetComponentInChildren<Canvas>().sortingOrder = costCardList.Count - 1;
        //残った手札の位置を調整
        ResetCardPosition();
    }

    /// <summary>
    /// 使用カードや使用コストを手札に戻す
    /// </summary>
    /// <param name="card">戻すカードオブジェクト</param>
    /// <param name="IsCost">コストか使用カードか</param>
    public void ResetToHand(Card card, bool IsCost)
    {
        //コストカードを戻す場合
        if (IsCost)
        {
            //コストリストから削除する
            costCardList.Remove(card);
            //戻すカードによって、支払い済みのコストの値を減らす
            switch(card.ID)
            {
            case 11: //サクリファイス・レプリカ
                PayedCost -= 2;
                break;

            default:
                PayedCost--;
                break;
            }
            if (SelectedCard != null && SelectedCard.Cost > PayedCost)
            {
                CardUseButton.SetActive(false);
            }
        }
        //手札リストに追加する
        _player.hands.Add(card);
        //手札置き場を親オブジェクトにする
        card.transform.SetParent(_player.playerHand);
        Vector3 movePoint = new Vector3(-315 + 70 * (_player.hands.Count - 1), 0, 0);
        //カードをスライド移動させる
        card.transform.DOLocalMove(movePoint, 0.1f, true);
        //何枚目の手札かによってカードオブジェクトにあるキャンバスの表示順を変更する
        card.GetComponentInChildren<Canvas>().sortingOrder = _player.hands.Count - 1;
        //使用コストとして選択されているカードの位置を調整
        ResetCostPosition();
        if (!IsCost)
        {
            SelectedCard = null;
            //カード使用ボタンの表示を解除する
            CardUseButton.SetActive(false);
            card.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f) * 1.1f;
        }
    }

    /// <summary>
    /// 手札のカードの位置を調整する関数
    /// </summary>
    public void ResetCardPosition()
    {
        for (int num = 0; num < _player.hands.Count; num++)
        {
            //何枚目の手札かによってカードオブジェクトにあるキャンバスの表示順を変更する
            _player.hands[num].GetComponentInChildren<Canvas>().sortingOrder = num;
            _player.hands[num].transform.localPosition = new Vector3(-315 + 70 * num, 0, 0);
        }
    }

    /// <summary>
    /// 手札にカーソルが重なっているカードが存在する場合に手札のカードの位置を調整する関数
    /// </summary>
    public void ResetCardPositionWhenWatching()
    {
        bool StartReset = false;
        //順番に手札リストにあるカードを調べる
        for (int num = 0; num < _player.hands.Count; num++)
        {
            //タグがついているカードが存在する場合
            if (_player.hands[num].tag == "Watching" && StartReset == false)
            {
                //カードの拡大によって位置を調整
                _player.hands[num].transform.localPosition = new Vector3(-315 + 70 * num + 14, 0, 0);
                //このカードが最後のカードじゃなかった場合、後ろのカードの位置の調整を開始する
                if (num != _player.hands.Count)
                {
                    StartReset = true;
                }
            }
            else if (StartReset == true)
            {
                _player.hands[num].transform.localPosition = new Vector3(-315 + 70 * num + 206, 0, 0);
            }
        }
    }

    /// <summary>
    /// 使用コストのカードの位置を調整する関数
    /// </summary>
    public void ResetCostPosition()
    {
        for (int num = 0; num < costCardList.Count; num++)
        {
            //何枚目のコストかによってカードオブジェクトにあるキャンバスの表示順を変更する
            costCardList[num].GetComponentInChildren<Canvas>().sortingOrder = num;
            costCardList[num].transform.localPosition = new Vector3(0, 95 - 70 * num, 0);
        }
    }

    /// <summary>
    /// 使用コストにカーソルが重なっているカードが存在する場合に使用コストのカードの位置を調整する関数
    /// </summary>
    public void ResetCostPositionWhenWatching()
    {
        bool StartReset = false;
        //順番にコストリストにあるカードを調べる
        for (int num = 0; num < costCardList.Count; num++)
        {
            //タグがついているカードが存在する場合
            if (costCardList[num].tag == "Watching" && StartReset == false)
            {
                //カードの拡大によって位置を調整
                costCardList[num].transform.localPosition = new Vector3(0, 95 - 70 * num - 19, 0);
                //このカードが最後のカードじゃなかった場合、後ろのカードの位置の調整を開始する
                if (num != costCardList.Count)
                {
                    StartReset = true;
                }
            }
            else if (StartReset == true)
            {
                costCardList[num].transform.localPosition = new Vector3(0, 95 - 70 * num - 322, 0);
            }
        }
    }

    /// <summary>
    /// 使用カードボタンが押された時の処理
    /// </summary>
    public void UseButtonOnClick()
    {
        SoundManager.Instance.PlaySE(3);
        //カード効果処理フェイズに移行する
        gameState = GameState.effect;
        //使用カードボタンの表示を解除する
        CardUseButton.SetActive(false);
    }
}