using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameDirector : SingletonMonoBehaviour<GameDirector>
{
    [SerializeField, Header("基本スコア")] public int point = 0;

    [SerializeField, Header("接地中に配置を確定するまでの時間")]
    private float _marginTime = 0;
    [SerializeField, Header("ミノの初期位置")] public Vector3 _DEFAULT_POSITION = Vector3.zero;
    [SerializeField] MeteorGeneretor _generator = null;
    //[SerializeField] private Player_1 _player1 = null;

    // プレイヤーのゲッタ
    //public Player_1 player1 => _player1;

    // ピース用リストを宣言
    public List<Meteorite> meteors = new List<Meteorite>();

    private int _turnCount = 0;
    private float _timeCount = 0;
    private bool _canChangeTurn = false;
    private bool _isDown = true;
    public GameObject[] _activePieces = new GameObject[2];
    public float intervalTime = 0;
    public bool _isLanding = false;
    public GameState gameState = GameState.none;
    public GameState nextStateCue = GameState.none;
    [SerializeField] draw _cardGenerator = null;

    public enum GameState
    {
        none,
        standby,
        active,
        confirmed,
        falled,
        reversed,
        idle,
        end,
        ended,
    }

    void Start()
    {
        //_player1.isMyTurn = false;
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
                    _cardGenerator.Draw();
                }

                //盤面の上4列に隕石を生成
                //横一列につき、隕石を二個ランダムに生成
                for (int i = 0; i < 4; i++)
                {
                    MeteorSet(2,i);
                }
                //ドローと隕石の処理が終了したら、アクティブフェイズに移行する
                gameState = GameState.active;
                break;

            case GameState.active: //アクティブフェイズ
                //プレイヤーの操作を受け付ける

                //プレイヤーの選択に応じて、処理フェイズに移行する
                gameState = GameState.confirmed;
                break;

            case GameState.confirmed: //処理フェイズ
                //ドロー処理

                //カード使用処理

                //特異遺物使用処理

                //処理終了後、隕石落下フェイズに移行する
                gameState = GameState.falled;
                break;

            case GameState.falled: //隕石落下フェイズ
                
                break;

            case GameState.reversed:

                break;

            case GameState.end: //ゲーム終了処理
                
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
            Vector3 checkPos = _DEFAULT_POSITION + new Vector3(x*180, -columns*169);
            //if (Map.Instance.CheckEmpty(checkPos))
            //{
                _generator.Generate(checkPos);
                created++;
            //}
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
}
