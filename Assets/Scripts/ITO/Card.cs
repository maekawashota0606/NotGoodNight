using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Card : CardData
{
    //[SerializeField, Header("カード選択フレーム")] private Image image_component = null;
    //マウスがカードの上に乗っているかどうか
    public bool IsMouseOver = false;
    //クリックされた状態なのかどうか
    public bool IsClick = false;
    //このカードがコストとして選択されているのかどうか
    public bool IsCost = false;
    private bool IsSEPlayed = false;

    void Update()
    {
        base.ShowCardStatus();

        if (GameDirector.Instance.WaitCopy_Card13 == true && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            GameDirector.Instance.CopyNum_Card13 = this.ID;
            //image_component.color = Color.white;
        }

        //ConfirmUsing();
        //WaitForSelect();
    }

    /// <summary>
    /// 選択されていない待機状態の処理
    /// </summary>
    private void WaitForSelect()
    {
        //このカードが選択されていない場合
        if (IsClick == false)
        {
            if (GameDirector.Instance.WaitCopy_Card13 == false)
            {
                //マウスが乗っていたら、カードの枠を黄色にする
                if (IsMouseOver == true)
                {
                    //image_component.color = Color.yellow;
                }
                //乗っていない場合、白色にする
                else
                {
                    //image_component.color = Color.white;
                }
            }
            //複製魔法の対象の選択
            else if (GameDirector.Instance.WaitCopy_Card13 == true)
            {
                //マウスが乗っていたら、カードの枠をマゼンタにする
                if (IsMouseOver == true)
                {
                    //image_component.color = Color.magenta;
                }
                //乗っていない場合、白色にする
                else
                {
                    //image_component.color = Color.white;
                }
            }
        }
    }

    /// <summary>
    /// 左クリックされた時の処理
    /// </summary>
    public void OnClick()
    {
        if (GameDirector.Instance.gameState != GameDirector.GameState.effect)
        {
            //使用カードとして選択された時の処理
            if (GameDirector.Instance.SelectedCard == null)
            {
                SoundManager.Instance.PlaySE(2);
                //image_component.color = Color.red;
                IsClick = true;
                GameDirector.Instance.SetSelectCard(this);
            }
            //コストカードとして選択された時の処理
            else if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost && IsClick == false)
            {
                SoundManager.Instance.PlaySE(2);
                //image_component.color = Color.green;
                IsClick = true;
                IsCost = true;
                GameDirector.Instance.SetCostCard(this);
            }
        }
    }

    /// <summary>
    /// カードの使用を確定する処理
    /// </summary>
    private void ConfirmUsing()
    {
        if (GameDirector.Instance.SelectedCard == null)
        {
            return;
        }
        else
        {
            if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCard.Cost && IsClick == true && IsCost == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
            {
                if (GameDirector.Instance.SelectedCard.ID == 11 || GameDirector.Instance.SelectedCard.ID == 15 || GameDirector.Instance.SelectedCard.ID == 19 || (GameDirector.Instance.SelectedCard.ID == 35 && Player.hands.Count != 1))
                {
                    return;
                }
                else
                {
                    SoundManager.Instance.PlaySE(3);
                    GameDirector.Instance.IsCardUsingConfirm = true;
                }
            }
        }
    }

    /// <summary>
    /// マウスがカードの上に乗っている時
    /// </summary>
    public void OnPointerEnter()
    {
        if (IsSEPlayed == false)
        {
            SoundManager.Instance.PlaySE(1);
            IsSEPlayed = true;
        }
        transform.localScale = Vector3.one * 1.1f;
        GetComponentInChildren<Canvas>().sortingLayerName = "Overlay";
        IsMouseOver = true;
        GameDirector.Instance.WatchingCard = this;
        this.tag = "Watching";
        GameDirector.Instance.ResetCardPositionWhenWatching();
        GameDirector.Instance.ResetCostPositionWhenWatching();
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    public void OnPointerExit()
    {
        transform.localScale = Vector3.one;
        GetComponentInChildren<Canvas>().sortingLayerName = "Card";
        IsSEPlayed = false;
        IsMouseOver = false;
        GameDirector.Instance.WatchingCard = null;
        this.tag = "Untagged";
        GameDirector.Instance.ResetCardPosition();
        GameDirector.Instance.ResetCostPosition();
    }

    /// <summary>
    /// マウスで右クリックされた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            if (IsClick && !IsCost)
            {
                GameDirector.Instance.ResetToHand(this, IsCost);
                if (GameDirector.Instance.PayedCost > 0)
                {
                    while (GameDirector.Instance.costCardList.Count > 0)
                    {
                        GameDirector.Instance.costCardList[0].IsClick = false;
                        GameDirector.Instance.costCardList[0].IsCost = false;
                        GameDirector.Instance.ResetToHand(GameDirector.Instance.costCardList[0], true);
                    }
                }
                IsClick = false;
                GameDirector.Instance.SelectedCard = null;
                GameDirector.Instance.IsBasePointInArea = true;
                GameDirector.Instance.PayedCost = 0;
            }
            else if (IsClick && IsCost)
            {
                GameDirector.Instance.ResetToHand(this, IsCost);
                IsClick = false;
                IsCost = false;
                switch(this.ID)
                {
                case 11: //サクリファイス・レプリカ
                    GameDirector.Instance.PayedCost -= 2;
                    break;

                default:
                    GameDirector.Instance.PayedCost--;
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        //チェンジリング・マギアの効果
        //コストとして使われた場合、ランダムに隕石を二個破壊する
        if (IsCost == true)
        {
            switch(this.ID)
            {
            case 15: //チェンジリング・マギア
                for (int i = 0; i < 2; i++)
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
            
            case 19: //魔力障壁
                GameDirector.Instance.DoMeteorFall = false;
                GameDirector.Instance.CanMeteorGenerate = false;
                Player.EffectTurn_Card19 = 1;
                break;

            default:
                break;
            }
        }
    }
}