using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Card : CardData
{
    //マウスがカードの上に乗っているかどうか
    public bool IsMouseOver = false;
    //クリックされた状態なのかどうか
    public bool IsClick = false;
    //このカードがコストとして選択されているのかどうか
    public bool IsCost = false;
    private bool IsSEPlayed = false;
    private Vector3 originPosition = Vector3.zero;
    private Vector3 mouse = Vector3.zero;
    private Vector3 target = Vector3.zero;
    [SerializeField] private Button buttonComponent = null;

    void Update()
    {
        base.ShowCardStatus();

        mouse = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));

        if (GameDirector.Instance.WaitCopy_Card13 == true && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            GameDirector.Instance.CopyNum_Card13 = this.ID;
        }
    }

    /// <summary>
    /// 左クリックされた時の処理
    /// </summary>
    public void OnClick()
    {
        Debug.Log("Clicked");
        SoundManager.Instance.PlaySE(2);
        if (GameDirector.Instance.gameState != GameDirector.GameState.effect)
        {
            //使用カードとして選択された時の処理
            if (GameDirector.Instance.SelectedCard == null)
            {
                if (this.ID == 11 || this.ID == 15 || this.ID == 19 || (this.ID == 35 && GameDirector.Instance._player.hands.Count != 1))
                {
                    return;
                }
                IsClick = true;
                GameDirector.Instance.SetSelectCard(this);
            }
            //コストカードとして選択された時の処理
            else if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCard.Cost && IsClick == false)
            {
                IsClick = true;
                IsCost = true;
                GameDirector.Instance.SetCostCard(this);
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
        //transform.localScale = Vector3.one * 1.1f;
        transform.localScale *= 1.1f;
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
        //transform.localScale = Vector3.one;
        transform.localScale /= 1.1f;
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
                GameDirector.Instance.PayedCost = 0;
            }
            else if (IsClick && IsCost)
            {
                GameDirector.Instance.ResetToHand(this, IsCost);
                IsClick = false;
                IsCost = false;
            }
        }
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        originPosition = transform.position;
        buttonComponent.enabled = false;
    }

    public void OnDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        transform.position = target;
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        bool flg = true;

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            if (GameDirector.Instance.SelectedCard == null && hit.gameObject.CompareTag("SelectedCardSpace"))
            {
                GameDirector.Instance.SetSelectCard(this);
                IsClick = true;
                flg = false;
            }
            else if (GameDirector.Instance.SelectedCard != null && IsClick == false && hit.gameObject.CompareTag("CostCardSpace"))
            {
                GameDirector.Instance.SetCostCard(this);
                IsClick = true;
                IsCost = true;
                flg = false;
            }
            else if (IsClick == true && hit.gameObject.CompareTag("PlayerHand"))
            {
                GameDirector.Instance.ResetToHand(this, IsCost);
                if (!IsCost && GameDirector.Instance.PayedCost > 0)
                {
                    while (GameDirector.Instance.costCardList.Count > 0)
                    {
                        GameDirector.Instance.costCardList[0].IsClick = false;
                        GameDirector.Instance.costCardList[0].IsCost = false;
                        GameDirector.Instance.ResetToHand(GameDirector.Instance.costCardList[0], true);
                    }
                    GameDirector.Instance.SelectedCard = null;
                    GameDirector.Instance.PayedCost = 0;
                }
                IsClick = false;
                IsCost = false;
                flg = false;
            }
        }

        if (flg)
        {
            transform.position = originPosition;
        }
        buttonComponent.enabled = true;
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