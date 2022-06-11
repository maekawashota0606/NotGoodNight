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
    [SerializeField] Player _player = null;
    
    // 隕石用リストを宣言
    public List<GameObject> meteors = new List<GameObject>();
    //ゲームの進行状況
    public GameState gameState = GameState.none;
    public GameState nextStateCue = GameState.none;
    //プレイヤーの行動を受け付けるかどうか
    public bool CanPlayerControl = false;
    //プレイヤーが行動を選択したかどうか
    public bool IsPlayerSelectMove = false;
    //隕石を生成するかどうか
    public bool CanMeteorGenerate = true;
    //ターンチェンジで生成する隕石の数
    private int MeteorGenNum = 2;
    //ターンカウント
    private int TurnCount = 0;
    //勝敗判定用フラグ
    public bool IsPlayerWin = false;
    //カード使用関連のフラグ
    //使用するカードが選択されているかどうか
    public bool IsCardSelect = false;
    //隕石の検索を行うかどうか
    public bool NeedSearch = false;
    //カード効果が処理されたのかどうか
    public bool IsCardUsed = false;
    //コストを支払う必要があるかどうか
    public bool NeedPayCost = false;
    //選択されているカードの使用に必要なコストの数
    public int NeedCost = 0;
    //すでに選択されているコストの数
    public int PayedCost = 0;
    
    public enum GameState
    {
        none,
        standby,
        active,
        confirmed,
        fall,
        judge,
        end,
        ended,
    }

    void Start()
    {
        //色々初期化
        CanPlayerControl = false;
        IsPlayerSelectMove = false;
        CanMeteorGenerate = true;
        IsPlayerWin = false;
        NeedPayCost = false;
        MeteorGenNum = 2;
        TurnCount = 0;
        NeedCost = 0;
        PayedCost = 0;
        IsCardSelect = false;
        gameState = GameState.standby;
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
                //プレイヤーの操作を受け付ける
                CanPlayerControl = true;
                //プレイヤーの行動を受け付けたら、隕石落下フェイズに移行する
                if (IsPlayerSelectMove == true)
                {
                    gameState = GameState.fall;
                }
                //ここらへんα版用
                /*
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _player.Score += 10000;
                }
                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    for (int num = 0; num < meteors.Count; num++)
                    {
                        var x = (int)meteors[num].transform.position.x;
                        var z = (int)meteors[num].transform.position.z * -1;
                        //隕石オブジェクトを削除する
                        Destroy(meteors[num]);
                        //リストから削除
                        meteors.RemoveAt(num);
                        //マップから削除
                        Map.Instance.map[z, x] = Map.Instance.empty;
                        num--;
                    }
                    gameState = GameState.judge;
                }
                */
                break;

            case GameState.fall: //隕石落下フェイズ
                IsPlayerSelectMove = false;
                for (int num = 0; num < meteors.Count; num++)
                {
                    var x = (int)meteors[num].transform.position.x;
                    var z = (int)meteors[num].transform.position.z * -1;
                    //隕石の下１マスが空白だった場合
                    if (z < 9)
                    {
                        //隕石オブジェクトを１マス下に移動
                        meteors[num].transform.position += Vector3.back;
                        //マップの元居た場所の記録を削除し
                        Map.Instance.map[z, x] = Map.Instance.empty;
                        //マップの移動先に新たに記録を書き込む
                        Map.Instance.map[z+1, x] = Map.Instance.meteor;
                    }
                    //隕石の下１マスが空白ではなかった場合
                    //下から順に処理を行っているため、隕石の下に他の隕石が存在する事はありえない
                    else if (z == 9)
                    {
                        //隕石オブジェクトを削除する
                        Destroy(meteors[num]);
                        //リストから削除
                        meteors.RemoveAt(num);
                        //マップから削除
                        Map.Instance.map[z, x] = Map.Instance.empty;
                        //プレイヤーのライフを減らす
                        _player.Life--;
                        num--;
                    }
                }
                gameState = GameState.judge;
                break;

            case GameState.judge: //ゲーム終了判定
                //プレイヤーのライフが0以下の場合、ゲームを終了させる
                if (_player.Life <= 0)
                {
                    gameState = GameState.end;
                }
                //プレイヤーのスコアが100000以上の場合
                else if (_player.Score >= 100000)
                {
                    //かつマップ上に隕石が存在しない場合、ゲームを終了させる
                    if (Map.Instance.CheckMap())
                    {
                        gameState = GameState.end;
                    }
                    //マップ上に隕石が存在する場合、隕石の生成を終了させる
                    else
                    {
                        CanMeteorGenerate = false;
                        gameState = GameState.active;
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
                    //ターンカウントを１つ増やす
                    TurnCount++;
                    //１０ターンごとに生成する隕石の数を１個増やす（上限は６個）
                    if (TurnCount % 10 == 0 && MeteorGenNum < 6)
                    {
                        MeteorGenNum++;
                    }
                    //アクティブフェイズに戻る
                    gameState = GameState.active;
                }
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
                gameState = GameState.ended;
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 隕石生成
    /// </summary>
    /// <param name="amount">生成する数</param>
    /// <param name="columns">生成する列</param>
    private void MeteorSet(int amount, int columns)
    {
        //生成した隕石の数
        int created = 0; 
        //生成を試みた回数
        int tryNum = 0;

        while (true)
        {
            //生成する場所を乱数で出す
            int x = Random.Range(0,9); 
            Vector3 checkPos = _DEFAULT_POSITION + new Vector3(x, 0, -columns);
            if (Map.Instance.CheckEmpty(checkPos))
            {
                _generator.Generate(checkPos);
                created++;
                Map.Instance.map[columns, x] = Map.Instance.meteor;
            }
            tryNum++;

            if (created == amount)
            {
                break;
            }
            
            if (tryNum == 10)
            {
                Debug.Log("生成できる場所が存在しない");
                break;
            }
        }
    }

    /// <summary>
    /// 隕石の破壊
    /// </summary>
    /// <param name="x">中心点のx座標</param>
    /// <param name="z">中心点のz座標</param>
    public void MeteorDestory(int x, int z)
    {
        for (int i = 0; i < meteors.Count; i++)
        {
            Debug.Log("searching meteor" + x + z);
            if (meteors[i].transform.position.x == x && meteors[i].transform.position.z == z * -1)
            {
                Debug.Log("Destory");
                Destroy(meteors[i]);
                //リストから削除
                meteors.RemoveAt(i);
                //マップから削除
                Map.Instance.map[z, x] = Map.Instance.empty;
                _player.Score += 1000;
                IsCardUsed = true;
                CanPlayerControl = false;
                IsPlayerSelectMove = true;
            }
        }
    }
}