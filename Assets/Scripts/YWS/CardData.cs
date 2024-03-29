using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;
using DG.Tweening;

public class CardData : MonoBehaviour
{
    #region  カードステータス関係の変数
    public int ID; //カード番号
    public string Name; //カード名
    public int Cost; //カードコスト
    public enum CardType
    {
        Convergence, //収束
        Diffusion, //拡散
        Cost, //コスト
    }
    public CardType CardTypeValue; //カードタイプ
    public bool IsDestroyEffect = false; //破壊効果持ちかどうか
    public string EffectText; //効果テキスト
    private string UpdateText;
    #endregion

    #region カードオブジェクト上のUI関係の変数
    [SerializeField, Header("カード名")] private Text CardName = null;
    [SerializeField, Header("コスト")] private Text CardCost = null;
    [SerializeField, Header("カード枠")] private Image CardFrame = null;
    [SerializeField, Header("カード選択枠")] private Sprite[] _cardFrameImage = new Sprite[2];
    [SerializeField, Header("効果テキスト")] private Text CardEffectText = null;
    [SerializeField, Header("カードイラスト")] private Image CardIllustration = null;
    [SerializeField, Header("カードイラスト")] private Sprite[] _illustrationImage = new Sprite[35];
    #endregion

    /// <summary>
    /// カード情報の初期化
    /// </summary>
    /// <param name="cardID">カード番号</param>
    public void Init(int cardID)
    {
        this.ID = cardID;
        this.Name = GameDirector.Instance._player._cardData[cardID][1];
        this.Cost = int.Parse(GameDirector.Instance._player._cardData[cardID][2]);
        if (GameDirector.Instance._player._cardData[cardID][3] == "0")
        {
            this.CardTypeValue = CardType.Convergence;
        }
        else if (GameDirector.Instance._player._cardData[cardID][3] == "1")
        {
            this.CardTypeValue = CardType.Diffusion;
        }
        else if (GameDirector.Instance._player._cardData[cardID][3] == "2")
        {
            this.CardTypeValue = CardType.Cost;
        }
        if (GameDirector.Instance._player._cardData[cardID][4] == "0")
        {
            IsDestroyEffect = true;
        }
        else if (GameDirector.Instance._player._cardData[cardID][4] == "1")
        {
            IsDestroyEffect = false;
        }
        this.EffectText = GameDirector.Instance._player._cardData[cardID][5].Replace("n","\n");

        GetComponentInChildren<Canvas>().sortingLayerName = "Card";

        if (CardName.color.a == 0)
        {
            FadeIn();
        }
    }

    /// <summary>
    /// カードオブジェクトに情報を反映する
    /// </summary>
    public void ShowCardStatus()
    {
        CardName.text = Name;
        CardCost.text = Cost.ToString();
        if (CardTypeValue == CardType.Convergence)
        {
            CardFrame.sprite = _cardFrameImage[0];
        }
        else if (CardTypeValue == CardType.Diffusion)
        {
            CardFrame.sprite = _cardFrameImage[1];
        }
        else if (CardTypeValue == CardType.Cost)
        {
            CardFrame.sprite = _cardFrameImage[2];
        }
        UpdateText = EffectText.Replace("x",GameDirector.Instance._player.DrawCount_Card10.ToString());
        CardEffectText.text = UpdateText;
        if (_illustrationImage[ID-1] != null)
            CardIllustration.sprite = _illustrationImage[ID-1];
    }

    /// <summary>
    /// カード効果の範囲を決める
    /// </summary>
    /// <param name="basicPosX">原点となるマスのx座標</param>
    /// <param name="basicPosZ">原点となるマスのz座標</param>
    public void DecideSearchArea(int basicPosX, int basicPosZ)
    {
        //カードの種類に基づいて、範囲となるマスすべてにタグを付ける
        switch(this.ID)
        {
        #region サラマンダーブレス
        case 1:
            if (basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            break;
        #endregion

        #region ウンディーネ・ウェイブ
        case 2:
            if (basicPosX > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region シルフ・ゲイル
        case 3:
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region ノーム・グレイブル
        case 4:
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;
        #endregion

        #region 星磁力
        case 6:
            if (basicPosZ > 0)
            {
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            }
            if (basicPosX < 9)
            {
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            }
            if (basicPosX < 9 && basicPosZ > 0)
            {
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            }
            break;
        #endregion

        #region グラビトンコア
        case 7:
            if (basicPosZ > 0 && basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;
        #endregion

        #region 彗星
        case 8:
            for (int i = basicPosX-1; i > -1; i--) //←
            {
                TileMap.Instance.tileMap[i, basicPosZ].tag = "Area";
            }
            for (int i = basicPosX+1; i < 10; i++) //→
            {
                TileMap.Instance.tileMap[i, basicPosZ].tag = "Area";
            }
            break;
        #endregion

        #region グラビトンブレイク
        case 9:
            Vector3 checkPos = new Vector3(basicPosX, 0, -basicPosZ);
            if (!Map.Instance.CheckEmpty(checkPos))
            {
                while (true)
                {
                    Vector3 basicPos = new Vector3(TileMap.Instance.checkListX[0], 0, -TileMap.Instance.checkListZ[0]);
                    Vector3 UpPos = basicPos + Vector3.forward;
                    Vector3 DownPos = basicPos + Vector3.back;
                    Vector3 LeftPos = basicPos + Vector3.left;
                    Vector3 RightPos = basicPos + Vector3.right;

                    if (-(int)UpPos.z > -1)
                    {
                        if (TileMap.Instance.tileMap[(int)UpPos.x, -(int)UpPos.z].CompareTag("Untagged") && !Map.Instance.CheckEmpty(UpPos))
                        {
                            TileMap.Instance.tileMap[(int)UpPos.x, -(int)UpPos.z].tag = "Area";
                            TileMap.Instance.checkListX.Add((int)UpPos.x);
                            TileMap.Instance.checkListZ.Add(-(int)UpPos.z);
                        }
                    }
                    if (-(int)DownPos.z < 10)
                    {
                        if (TileMap.Instance.tileMap[(int)DownPos.x, -(int)DownPos.z].CompareTag("Untagged") && !Map.Instance.CheckEmpty(DownPos))
                        {
                            TileMap.Instance.tileMap[(int)DownPos.x, -(int)DownPos.z].tag = "Area";
                            TileMap.Instance.checkListX.Add((int)DownPos.x);
                            TileMap.Instance.checkListZ.Add(-(int)DownPos.z);
                        }
                    }
                    if ((int)LeftPos.x > -1)
                    {
                        if (TileMap.Instance.tileMap[(int)LeftPos.x, -(int)LeftPos.z].CompareTag("Untagged") && !Map.Instance.CheckEmpty(LeftPos))
                        {
                            TileMap.Instance.tileMap[(int)LeftPos.x, -(int)LeftPos.z].tag = "Area";
                            TileMap.Instance.checkListX.Add((int)LeftPos.x);
                            TileMap.Instance.checkListZ.Add(-(int)LeftPos.z);
                        }
                    }
                    if ((int)RightPos.x < 10)
                    {
                        if (TileMap.Instance.tileMap[(int)RightPos.x, -(int)RightPos.z].CompareTag("Untagged") && !Map.Instance.CheckEmpty(RightPos))
                        {
                            TileMap.Instance.tileMap[(int)RightPos.x, -(int)RightPos.z].tag = "Area";
                            TileMap.Instance.checkListX.Add((int)RightPos.x);
                            TileMap.Instance.checkListZ.Add(-(int)RightPos.z);
                        }
                    }

                    TileMap.Instance.checkListX.RemoveAt(0);
                    TileMap.Instance.checkListZ.RemoveAt(0);

                    if (TileMap.Instance.checkListX.Count == 0 && TileMap.Instance.checkListZ.Count == 0)
                    {
                        break;
                    }
                }
            }
            break;
        #endregion

        #region コメットブロー
        case 12:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosZ > 0 && basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;
        #endregion

        #region アストラルリベリオン
        case 20:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 1 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ-2].tag = "Area"; //←↑←↑
            if (basicPosZ > 0 && basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosX > 0 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-2].tag = "Area"; //↑↑←
            if (basicPosX < 9 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-2].tag = "Area"; //↑↑→
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 8 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ-2].tag = "Area"; //→↑→↑
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 8)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            if (basicPosX < 8 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ-1].tag = "Area"; //→→↑
            if (basicPosX < 8 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ+1].tag = "Area"; //→→↓
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 8 && basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ+2].tag = "Area"; //→↓→↓
            if (basicPosX < 9 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            if (basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+2].tag = "Area"; //↓↓
            if (basicPosX < 9 && basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+2].tag = "Area"; //↓↓→
            if (basicPosX > 0 && basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ+2].tag = "Area"; //↓↓←
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 1 && basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ+2].tag = "Area"; //←↓←↓
            if (basicPosX > 0 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 1 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ+1].tag = "Area"; //←←↓
            if (basicPosX > 1 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ-1].tag = "Area"; //←←↑
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            break;
        #endregion

        #region グラビトンオフセッツ
        case 23:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 8)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            break;
        #endregion

        #region 知性の光
        case 25:
            GameDirector.Instance.IsMultiEffect = true;
            if (basicPosX > 1 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ-2].tag = "Area"; //←↑←↑
            if (basicPosX > 0 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosX < 9 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            break;
        #endregion

        #region 願いの代償
        case 26:
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region ドラゴニックブレス
        case 29:
            if (basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX < 9 && basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-2].tag = "Area"; //→↑↑
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX < 9 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ+1].tag = "Area"; //→↓
            break;
        #endregion

        #region タイダルウェーブ
        case 30:
            if (basicPosX > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            if (basicPosX > 1 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ-1].tag = "Area"; //←←↑
            if (basicPosX > 0 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;
        #endregion

        #region シルフ・サイクロン
        case 31:
            if (basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosZ < 8)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+2].tag = "Area"; //↓↓
            if (basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX, basicPosZ+1].tag = "Area"; //↓
            if (basicPosX > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosX < 8)
                TileMap.Instance.tileMap[basicPosX+2, basicPosZ].tag = "Area"; //→→
            if (basicPosX < 9)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ].tag = "Area"; //→
            break;
        #endregion

        #region テラリウムグレイブ
        case 32:
            if (basicPosX > 1)
                TileMap.Instance.tileMap[basicPosX-2, basicPosZ].tag = "Area"; //←←
            if (basicPosX > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ].tag = "Area"; //←
            if (basicPosZ > 1)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-2].tag = "Area"; //↑↑
            if (basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX, basicPosZ-1].tag = "Area"; //↑
            if (basicPosX > 0 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ-1].tag = "Area"; //←↑
            if (basicPosX > 0 && basicPosZ < 9)
                TileMap.Instance.tileMap[basicPosX-1, basicPosZ+1].tag = "Area"; //←↓
            if (basicPosX < 9 && basicPosZ > 0)
                TileMap.Instance.tileMap[basicPosX+1, basicPosZ-1].tag = "Area"; //→↑
            break;
        #endregion

        #region 次元断裂
        case 34:
            for (int num = -3; num < 3; num++)
            {
                int upX = basicPosX;
                int upZ = basicPosZ + num;
                int downX = basicPosX;
                int downZ = basicPosZ + num;
                for (int i = 0; i < 10; i++)
                {
                    if (upX >= 0 && upX <= 9 && upZ >= 0 && upZ <= 9)
                    {
                        if (TileMap.Instance.tileMap[upX,upZ].CompareTag("Untagged"))
                        {
                            TileMap.Instance.tileMap[upX, upZ].tag = "Area";
                        }
                    }
                    upX++;
                    upZ--;
                }
                for (int j = 0; j < 10; j++)
                {
                    if (downX >= 0 && downX <= 9 && downZ >= 0 && downZ <= 9)
                    {
                        if (TileMap.Instance.tileMap[downX, downZ].CompareTag("Untagged"))
                        {
                            TileMap.Instance.tileMap[downX, downZ].tag = "Area";
                        }
                    }
                    downX--;
                    downZ++;
                }
            }
            break;
        #endregion

        default:
            break;
        }
    }

    public void CardEffect()
    {
        switch(this.ID)
        {
        #region サラマンダーブレス
        case 1:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region ウンディーネ・ウェイブ
        case 2:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region シルフ・ゲイル
        case 3:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region ノーム・グレイブル
        case 4:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region アストラルリコール
        case 5:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < 4; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion
            
        #region 星磁力
        case 6:
            MeteorAttract();
            break;
        #endregion

        #region グラビトンコア
        case 7:
            MeteorAttract();
            break;
        #endregion

        #region 彗星
        case 8:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region グラビトンブレイク
        case 9:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion
            
        #region 星屑収集
        case 10:
            for (int i = 0; i < GameDirector.Instance._player.DrawCount_Card10; i++)
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

        #region コメットブロー
        case 12:
            TileMap.Instance.MeteorDestory();
            ExtraEffect();
            break;
        #endregion

        #region 複製魔法
        case 13:
            int[] CardID = new int[31]{1,2,3,4,5,6,7,8,9,10,12,14,16,17,18,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35};
            int ranNum = Random.Range(0,CardID.Length);
            int copyID = CardID[ranNum];
            GameDirector.Instance.gameState = GameDirector.GameState.extra;
            FadeOut(copyID);
            break;
        #endregion

        #region グラビトンリジェクト
        case 14:
            GameDirector.Instance._player.EffectTurn_Card14 = 3;
            GameDirector.Instance.DoMeteorFall = false;
            GameDirector.Instance.CanMeteorGenerate = false;
            break;
        #endregion

        #region 不破の城塞
        case 16:
            for (int i = 0; i < GameDirector.Instance._player.hands.Count+2; i++)
            {
                if (GameDirector.Instance.meteors.Count == 0 || GameDirector.Instance._player.hands.Count == 0)
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

        #region 聖櫃の開放
        case 17:
            for (int i = 0; i < GameDirector.Instance._player.hands.Count; i++)
            {
                GameDirector.Instance._player.hands[i].Cost = 0;
            }
            break;
        #endregion

        #region 詮索するはばたき
        case 18:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < 3; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region アストラルリベリオン
        case 20:
            TileMap.Instance.MeteorDestory();
            ExtraEffect();
            break;
        #endregion

        #region 残光のアストラル
        case 21:
            GameDirector.Instance._player.IsDrawEffect = true;
            int DrawNum = 2;
            DrawNum += Player.Score / 30000 * 2;
            for (int i = 0; i < DrawNum; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region 至高天の顕現
        case 22:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < 2; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region グラビトンオフセッツ
        case 23:
            TileMap.Instance.MeteorDestory();
            ExtraEffect();
            break;
        #endregion

        #region 隕石の儀式
        case 24:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int x = 0; x < 10; x++)
            {
                Vector3 checkPos = new Vector3(x, 0, 0);
                if (Map.Instance.CheckEmpty(checkPos))
                {
                    GameDirector.Instance.MeteorSetTarget(x,0);
                }
            }
            for (int i = 0; i < 6; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region 知性の光
        case 25:
            TileMap.Instance.MeteorDestory();
            ExtraEffect();
            break;
        #endregion

        #region 願いの代償
        case 26:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int z = 0; z < 10; z++)
            {
                for (int x = 0; x < 10; x++)
                {
                    if (TileMap.Instance.tileMap[x, z].CompareTag("Search") || TileMap.Instance.tileMap[x, z].CompareTag("Area"))
                    {
                        Vector3 checkPos = new Vector3(x, 0, -z);
                        if (Map.Instance.CheckEmpty(checkPos))
                        {
                            GameDirector.Instance.MeteorSetTarget(x,z);
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region 復興の灯
        case 27:
            GameDirector.Instance._player.Life++;
            break;
        #endregion

        #region 光の奔流
        case 28:
            GameDirector.Instance._player.EffectTurn_Card28 = 4;
            break;
        #endregion

        #region ドラゴニックブレス
        case 29:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region タイダルウェーブ
        case 30:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region シルフ・サイクロン
        case 31:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region テラリウムグレイブ
        case 32:
            TileMap.Instance.MeteorDestory();
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

        #region 次元断裂
        case 34:
            TileMap.Instance.MeteorDestory();
            break;
        #endregion

        #region ラスト・ショット
        case 35:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < 10; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        default:
            break;
        }
        GameDirector.Instance._player.IsDrawEffect = false;
    }

    /// <summary>
    /// 二個以上の効果を持つカードの効果処理
    /// </summary>
    public void ExtraEffect()
    {
        switch(this.ID)
        {
        #region コメットブロー
        case 12:
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < 5; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        #region アストラルリベリオン
        case 20:
            GameDirector.Instance._player.IsDrawEffect = true;
            if (Player.Score >= 50000)
            {
                for (int i = 0; i < 7; i++)
                {
                    GameDirector.Instance._player.DrawCard();
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
                    Vector3 checkPos = new Vector3(x, 0, -z);
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
            GameDirector.Instance._player.IsDrawEffect = true;
            for (int i = 0; i < GameDirector.Instance.DestroyedNum; i++)
            {
                GameDirector.Instance._player.DrawCard();
            }
            break;
        #endregion

        default:
            break;
        }
        GameDirector.Instance._player.IsDrawEffect = false;
        GameDirector.Instance.IsMultiEffect = false;
    }

    private void MeteorAttract()
    {
        GameDirector.Instance._player.MoveList = new List<Meteorite>();
        GameDirector.Instance._player.targetPosXList = new List<int>();
        GameDirector.Instance._player.targetPosZList = new List<int>();
        int LockedNum = 0;
        for (int x = 0; x < 10; x++)
        {
            for (int z = 0; z < 10; z++)
            {
                if (TileMap.Instance.tileMap[x, z].CompareTag("Search") || TileMap.Instance.tileMap[x, z].CompareTag("Area"))
                {
                    Vector3 checkPos = new Vector3(x, 0, -z);
                    if (!Map.Instance.CheckEmpty(checkPos))
                    {
                        TileMap.Instance.tileMap[x,z].tag = "Lock";
                        LockedNum++;
                    }
                    else
                    {
                        GameDirector.Instance._player.targetPosXList.Add(x);
                        GameDirector.Instance._player.targetPosZList.Add(z);
                    }
                }
            }
        }

        for (int num = 0; num < GameDirector.Instance._player.targetPosXList.Count; num++)
        {
            if (GameDirector.Instance.meteors.Count == 0 || GameDirector.Instance._player.MoveList.Count == 9 || GameDirector.Instance.meteors.Count == GameDirector.Instance._player.MoveList.Count + LockedNum)
            {
                break;
            }

            int chosenNum = Random.Range(0,GameDirector.Instance.meteors.Count);
            int checkx = (int)GameDirector.Instance.meteors[chosenNum].transform.position.x;
            int checkz = -(int)GameDirector.Instance.meteors[chosenNum].transform.position.z;
            if (TileMap.Instance.tileMap[checkx, checkz].tag != "Lock" && TileMap.Instance.tileMap[checkx, checkz].tag != "Move")
            {
                TileMap.Instance.tileMap[checkx,checkz].tag = "Move";
                GameDirector.Instance._player.MoveList.Add(GameDirector.Instance.meteors[chosenNum]);
            }
            else
            {
                num--;
            }
        }
        GameDirector.Instance.WaitingMove = true;
        for (int num = 0; num < GameDirector.Instance._player.MoveList.Count; num++)
        {
            GameDirector.Instance._player.MoveList[num].MoveToTargetPoint(GameDirector.Instance._player.targetPosXList[num], -GameDirector.Instance._player.targetPosZList[num]);
        }
    }

    public void FadeOut(int newID)
    {
        CardName.DOFade(0f, 0.5f);
        CardCost.DOFade(0f, 0.5f);
        CardFrame.DOFade(0f, 0.5f);
        CardEffectText.DOFade(0f, 0.5f);
        CardIllustration.DOFade(0f, 0.5f).OnComplete(()=>Init(newID));
    }

    public void FadeIn()
    {
        GameDirector.Instance.PayedCost = this.Cost;
        CardName.DOFade(1f, 0.5f);
        CardCost.DOFade(1f, 0.5f);
        CardFrame.DOFade(1f, 0.5f);
        CardEffectText.DOFade(1f, 0.5f);
        CardIllustration.DOFade(1f, 0.5f);
    }
}