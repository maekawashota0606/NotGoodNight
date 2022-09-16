using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Card : CardData
{
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
        //マウスカーソルの位置を随時更新する
        mouse = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
    }

    /// <summary>
    /// 左クリックされた時の処理
    /// </summary>
    public void OnClick()
    {
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
        //カードを少し拡大する
        transform.localScale *= 1.1f;
        //カードオブジェクトのキャンバスを一番上に表示させるための物に変更する
        GetComponentInChildren<Canvas>().sortingLayerName = "Overlay";
        GameDirector.Instance.WatchingCard = this;
        this.tag = "Watching";
        //カードの位置を調整
        GameDirector.Instance.ResetCardPositionWhenWatching();
        GameDirector.Instance.ResetCostPositionWhenWatching();
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    public void OnPointerExit()
    {
        //カードのサイズを元に戻す
        transform.localScale /= 1.1f;
        //カードオブジェクトのキャンバスを元に戻す
        GetComponentInChildren<Canvas>().sortingLayerName = "Card";
        IsSEPlayed = false;
        GameDirector.Instance.WatchingCard = null;
        this.tag = "Untagged";
        //カードの位置を調整
        GameDirector.Instance.ResetCardPosition();
        GameDirector.Instance.ResetCostPosition();
    }

    /// <summary>
    /// マウスで右クリックされた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(BaseEventData eventData)
    {
        //複製魔法処理フェイズの時、選択を解除できない
        if (GameDirector.Instance.gameState == GameDirector.GameState.extra)
        {
            return;
        }

        var pointerEventData = eventData as PointerEventData;
        //EventTriggerを使って右クリックを取得
        if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            //使用カードだった場合
            if (IsClick && !IsCost)
            {
                //カードを手札に戻す
                GameDirector.Instance.ResetToHand(this, IsCost);
                //コストが支払われていた場合
                if (GameDirector.Instance.PayedCost > 0)
                {
                    //コストリストのカード全てを手札に戻す
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
            //コストだった場合
            else if (IsClick && IsCost)
            {
                //このカードだけを手札に戻す
                GameDirector.Instance.ResetToHand(this, IsCost);
                IsClick = false;
                IsCost = false;
            }
        }
    }

    /// <summary>
    /// ドラッグを開始する関数
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(BaseEventData eventData)
    {
        //複製魔法処理フェイズの時、ドラッグを行えない
        if (GameDirector.Instance.gameState == GameDirector.GameState.extra)
        {
            return;
        }

        var pointerEventData = eventData as PointerEventData;
        //ドラッグ開始前の位置を記録する
        originPosition = transform.position;
        //ボタンのクリック判定を避けるため、一時的にカードオブジェクト上にあるボタンコンポーネントを無効にする
        buttonComponent.enabled = false;
    }

    /// <summary>
    /// ドラッグ途中の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        //マウスカーソルの位置にカードオブジェクトを追従させる
        transform.position = target;
    }

    /// <summary>
    /// ドラッグ終了時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        bool flg = true;

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            //使用カードが選択されておらず、このカードが使用カード置き場の上でドラッグを解除された場合
            if (GameDirector.Instance.SelectedCard == null && hit.gameObject.CompareTag("SelectedCardSpace"))
            {
                //このカードを使用カードとしてセットする
                GameDirector.Instance.SetSelectCard(this);
                IsClick = true;
                flg = false;
            }
            //使用カードが選択されており、このカードが使用コスト置き場の上でドラッグを解除された場合
            else if (GameDirector.Instance.SelectedCard != null && GameDirector.Instance.SelectedCard.Cost > GameDirector.Instance.PayedCost && IsClick == false && hit.gameObject.CompareTag("CostCardSpace"))
            {
                //このカードを使用コストとして登録する
                GameDirector.Instance.SetCostCard(this);
                IsClick = true;
                IsCost = true;
                flg = false;
            }
            //このカードが選択されていて、手札置き場の上でドラッグを解除された場合
            else if (IsClick == true && hit.gameObject.CompareTag("PlayerHand"))
            {
                //このカードを手札に戻す
                GameDirector.Instance.ResetToHand(this, IsCost);
                //使用カードでコストが支払われていた場合
                if (!IsCost && GameDirector.Instance.PayedCost > 0)
                {
                    //コストリストのカード全てを手札に戻す
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
        //使用カード置き場、使用コスト置き場、手札置き場のどれかでもない場所でドラッグを解除された場合、または条件が合わなかった場合、ドラッグ開始前の場所に戻す
        if (flg)
        {
            transform.position = originPosition;
        }
        buttonComponent.enabled = true;
    }

    /// <summary>
    /// このカードが削除される時の関数
    /// </summary>
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