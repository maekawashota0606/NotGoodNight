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

        //使用するカードが選択されており、それがこのカードではなく、かつ選択されているコストが必要コスト以下の場合、このカードがクリックされた時、枠を緑色にする
        if (GameDirector.Instance.PayedCost < GameDirector.Instance.NeedCost && GameDirector.Instance.IsCardSelect == true && IsClick == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
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
            this.tag = "Selected";
        }
        //カードが選択されていない場合、このカードがクリックされたら、枠を赤色にする
        else if (GameDirector.Instance.gameState != GameDirector.GameState.effect && GameDirector.Instance.IsCardSelect == false && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            image_component.color = Color.red;
            IsClick = true;
            GameDirector.Instance.IsCardSelect = true;
            //このカードの番号を報告する
            GameDirector.Instance.SelectedCardNum = this.ID;
            //このカードの使用に必要なコストの数を報告する
            GameDirector.Instance.NeedCost = this.Cost;
           
            //コストが0ではない場合、コストが必要だとフラグを立てる
            if (this.Cost != 0)
            {
                GameDirector.Instance.NeedPayCost = true;
            }
            if (this.CardTypeValue == CardType.Attack)
            {
                GameDirector.Instance.IsAttackCard = true;
            }
            //効果が処理された後に削除するために、タグを付けておく
            this.tag = "Selected";
        }

        if (GameDirector.Instance.gameState == GameDirector.GameState.effect && IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            GameDirector.Instance.IsMultiEffect = true;
            GameDirector.Instance.CopyNum_Card13 = this.ID;
        }

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
                GameDirector.Instance.IsAttackCard = false;
                GameDirector.Instance.IsBasePointInArea = true;
                GameDirector.Instance.IsCardSelect = false;
                GameDirector.Instance.SelectedCardNum = 0;
                GameDirector.Instance.NeedCost = 0;
                GameDirector.Instance.NeedPayCost = false;
                GameDirector.Instance.PayedCost = 0;
                this.tag = "Untagged";
            }
        }

        //コストとして選択されている、かつ使用カードの選択が解除された時、このカードの選択も解除する
        if (IsCost == true && GameDirector.Instance.IsCardSelect == false)
        {
            image_component.color = Color.white;
            IsClick = false;
            IsCost = false;
            this.tag = "Untagged";
        }

        //このカードが選択されていない場合
        if (IsClick == false)
        {
            if (GameDirector.Instance.gameState == GameDirector.GameState.effect)
            {
                //マウスが乗っていたら、カードの枠をマゼンタにする
                if (IsMouseOver == true)
                {
                    image_component.color = Color.magenta;
                }
                //乗っていない場合、白色にする
                else
                {
                    image_component.color = Color.white;
                }
            }
            else
            {
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
        GameDirector.Instance.SelectedCardObject = this;
        Debug.Log(GameDirector.Instance.SelectedCardObject.EffectText);
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

    public static implicit operator Card(EffectText2 v)
    {
        throw new System.NotImplementedException();
    }
}