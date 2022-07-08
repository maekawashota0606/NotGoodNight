using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Card : CardData
{
    [SerializeField, Header("カード選択フレーム")] private Image image_component = null;
    //マウスがカードの上に乗っているかどうか
    public bool IsMouseOver = false;
    //クリックされた状態なのかどうか
    public bool IsClick = false;
    //このカードがコストとして選択されているのかどうか
    public bool IsCost = false;

    void Update()
    {
        base.ShowCardStatus();

        ConfirmUsing();
        WaitForSelect();
        SelectingUseCard();
        PayingCost();
        UnChoosing();
        
        
        /*
        if (GameDirector.Instance.gameState == GameDirector.GameState.effect && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            GameDirector.Instance.IsMultiEffect = true;
            GameDirector.Instance.CopyNum_Card13 = this.ID;
        }
        */
    }

    /// <summary>
    /// 選択されていない待機状態の処理
    /// </summary>
    private void WaitForSelect()
    {
        //このカードが選択されていない場合
        if (IsClick == false)
        {
            //if (GameDirector.Instance.gameState == GameDirector.GameState.effect)
            //{
                //マウスが乗っていたら、カードの枠をマゼンタにする
                //if (IsMouseOver == true)
                //{
                    //image_component.color = Color.magenta;
                //}
                //乗っていない場合、白色にする
                //else
                //{
                    //image_component.color = Color.white;
                //}
            //}
            //else
            //{
                //マウスが乗っていたら、カードの枠を黄色にする
                if (IsMouseOver == true)
                {
                    image_component.color = Color.yellow;
                }
                //乗っていない場合、白色にする
                else
                {
                    image_component.color = Color.white;
                }
            //}
        }
    }

    /// <summary>
    /// 使用カードとして選択された時の処理
    /// </summary>
    private void SelectingUseCard()
    {
        //カードが選択されていない場合、このカードがクリックされたら、枠を赤色にする
        if (GameDirector.Instance.SelectedCardObject == null && GameDirector.Instance.gameState != GameDirector.GameState.effect && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            image_component.color = Color.red;
            IsClick = true;
            GameDirector.Instance.SelectedCardObject = this;
            if (this.IsBasePointInArea == false)
            {
                GameDirector.Instance.IsBasePointInArea = false;
            }
            //効果が処理された後に削除するために、タグを付けておく
            this.tag = "Selected";
        }
    }

    /// <summary>
    /// コストカードとして選択された時の処理
    /// </summary>
    private void PayingCost()
    {
        if (GameDirector.Instance.SelectedCardObject == null)
        {
            return;
        }
        else
        {
            //使用するカードが選択されており、それがこのカードではなく、かつ選択されているコストが必要コスト以下の場合、このカードがクリックされた時、枠を緑色にする
            if (GameDirector.Instance.PayedCost < GameDirector.Instance.SelectedCardObject.Cost && IsClick == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
            {
                image_component.color = Color.green;
                IsClick = true;
                IsCost = true;
                //選択されているコストの数を加算する
                switch(this.ID)
                {
                case 11: //サクリファイス・レプリカ
                    GameDirector.Instance.PayedCost += 2;
                    break;

                case 15: //チェンジリング・マギア
                    GameDirector.Instance.IsCostEffect = true;
                    break;
                
                case 19: //魔力障壁
                    GameDirector.Instance.IsCostEffect = true;
                    break;

                default:
                    GameDirector.Instance.PayedCost++;
                    break;
                }
                //コストとして使用された時に削除する用にタグを付けておく
                this.tag = "Cost";
            }
        }
    }

    /// <summary>
    /// 選択を解除する時の処理
    /// </summary>
    private void UnChoosing()
    {
        //このカードが選択されている場合で右クリックしたら、選択を解除し、枠を白色にする
        if (IsClick == true && IsMouseOver == true && Input.GetMouseButtonDown(1))
        {
            image_component.color = Color.white;
            IsClick = false;
            //コストとして選択されていた場合、すでに選択されているコストの数を減らす
            if (IsCost == true)
            {
                switch(this.ID)
                {
                case 11: //サクリファイス・レプリカ
                    GameDirector.Instance.PayedCost -= 2;
                    break;

                case 15: //チェンジリング・マギア
                    GameDirector.Instance.IsCostEffect = false;
                    break;
            
                case 19: //魔力障壁
                    GameDirector.Instance.IsCostEffect = false;
                    break;

                default:
                    GameDirector.Instance.PayedCost--;
                    break;
                }
            }
            //使用カードとして選択されていた場合、色々とリセットする
            else
            {
                GameDirector.Instance.SelectedCardObject = null;
                GameDirector.Instance.IsBasePointInArea = true;
                GameDirector.Instance.PayedCost = 0;
            }
            this.tag = "Untagged";
        }

        //コストとして選択されている、かつ使用カードの選択が解除された時、このカードの選択も解除する
        if (IsCost == true && GameDirector.Instance.SelectedCardObject == null)
        {
            image_component.color = Color.white;
            IsClick = false;
            IsCost = false;
            this.tag = "Untagged";
        }
    }

    /// <summary>
    /// カードの使用を確定する処理
    /// </summary>
    private void ConfirmUsing()
    {
        if (GameDirector.Instance.SelectedCardObject == null)
        {
            return;
        }
        else
        {
            if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.PayedCost >= GameDirector.Instance.SelectedCardObject.Cost && IsClick == true && IsCost == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
            {
                if (GameDirector.Instance.SelectedCardObject.ID == 11 || GameDirector.Instance.SelectedCardObject.ID == 15 && GameDirector.Instance.SelectedCardObject.ID == 19 || (GameDirector.Instance.SelectedCardObject.ID == 35 && Player.hands.Count != 1))
                {
                    return;
                }
                else
                {
                    GameDirector.Instance.IsCardUsingConfirm = true;
                }
            }
        }
    }

    /// <summary>
    /// マウスがカードの上に乗っている時
    /// </summary>
    private void OnMouseOver()
    {
        IsMouseOver = true;
        Debug.Log(this.ID + " " + this.Name);
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    private void OnMouseExit()
    {
        IsMouseOver = false;
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
                    int DestoryNum = Random.Range(0,GameDirector.Instance.meteors.Count);
                    //隕石オブジェクトを削除する
                    Destroy(GameDirector.Instance.meteors[DestoryNum]);
                    //リストから削除
                    GameDirector.Instance.meteors.RemoveAt(DestoryNum);
                    //マップから削除
                    Map.Instance.map[(int)GameDirector.Instance.meteors[DestoryNum].transform.position.z*-1, (int)GameDirector.Instance.meteors[DestoryNum].transform.position.x] = Map.Instance.empty;
                }
                break;
            
            case 19: //魔力障壁
                GameDirector.Instance.DoMeteorFall = false;
                GameDirector.Instance.IsEffect_Card19 = true;
                break;

            default:
                break;
            }
            GameDirector.Instance.IsCostEffect = false;
        }
    }
}