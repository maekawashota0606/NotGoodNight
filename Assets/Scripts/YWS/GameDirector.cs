using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameDirector : SingletonMonoBehaviour<GameDirector>
{
    [SerializeField, Header("基本獲得スコア")] public int point = 0;
    [SerializeField, Header("隕石の初期位置")] public Vector3 _DEFAULT_POSITION = Vector3.zero;
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
    //カーソルが合っているカードオブジェクト
    public Card WatchingCard = null;
    //使用カードとして選択されているカードオブジェクト
    public Card SelectedCard = null;
    //盤面にマウスカーソルが乗っているか
    public bool IsMouseOnTile = false;
    //基点マスに光って欲しいのかどうか
    public bool IsBasePointInArea = true;
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
        Debug.Log("Now Turn " + TurnCount);
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
                TileMap.Instance.FindBasePoint();
                //カードの使用が確定した場合
                if (IsCardUsingConfirm == true)
                {
                    //使用されたコストカードをすべて削除する
                    _player.DeleteUsedCost();
                    //カード効果処理フェイズに移行する
                    gameState = GameState.effect;
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
                    _player.DeleteUsedCard();
                    SelectedCard = null;
                    TileMap.Instance.ResetTileTag();
                    IsMeteorDestroyed = false;
                    //効果処理が終了したら、隕石落下フェイズに移行する
                    gameState = GameState.fall;
                }

                if (SelectedCard != null)
                {
                    TileMap.Instance.FindBasePoint();
                    if (SelectedCard.CardTypeValue == CardData.CardType.Attack)
                    {
                        if (IsMeteorDestroyed == true && IsMultiEffect == true)
                        {
                            _player.ExtraEffect();
                            IsPlayerSelectMove = true;
                        }
                    }
                    else if (SelectedCard.CardTypeValue == CardData.CardType.Special)
                    {
                        _player.SpecialCardEffect();
                        if (WaitCopy_Card13 == false)
                        {
                            IsPlayerSelectMove = true;
                        }
                    }
                }
                break;

            case GameState.fall: //隕石落下フェイズ
                if (SelectedCard == null)
                {
                    IsBasePointInArea = true;
                }
                IsPlayerSelectMove = false;
                if (DoMeteorFall == true)
                {
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
        gameState = GameState.standby;
        IsPlayerSelectMove = false;
        CanMeteorGenerate = true;
        MeteorGenNum = 2;
        TurnCount = 1;
        DoMeteorFall = true;
        IsPlayerWin = false;
        SelectedCard = null;
        IsMouseOnTile = false;
        IsBasePointInArea = true;
        IsMultiEffect = false;
        IsCardUsingConfirm = false;
        IsMeteorDestroyed = false;
        DestroyedNum = 0;
        PayedCost = 0;
        Player.hands = new List<Card>();
    }

    /// <summary>
    /// 隕石生成
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
            Vector3 checkPos = _DEFAULT_POSITION + new Vector3(x, 0, -columns);
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
    /// スコアの加算
    /// </summary>
    public void AddScore()
    {
        var GetScore = 1000 * GameDirector.Instance.DestroyedNum * (1 + GameDirector.Instance.DestroyedNum * 0.1f);
        _player.Score += (int)GetScore;
    }
}