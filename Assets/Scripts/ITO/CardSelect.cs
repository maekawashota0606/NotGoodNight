using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelect : MonoBehaviour
{
    //カードイラスト
    [SerializeField]
    Image image_component = null;
    //マウスがカードの上に乗っているかどうか
    public bool IsMouseOver = false;
    //クリックされた状態なのかどうか
    public bool IsClick = false;

    void Start()
    {
        
    }

    void Update()
    {
        //選択されていないカードをクリックしたら、そのカードの枠を赤色にする
        if (IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            image_component.color = Color.red;
            IsClick = true;
        }

        //選択されたカードを右クリックしたら、そのカードの選択を解除し、枠を白色にする
        if (IsMouseOver == true && Input.GetMouseButtonDown(1))
        {
            image_component.color = Color.white;
            IsClick = false;
        }

        //このカードが選択されたいない場合
        if (IsClick == false)
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

    /// <summary>
    /// マウスがカードの上に乗っている時
    /// </summary>
    void OnMouseOver()
    {
        IsMouseOver = true;
        //Debug.Log("OnMouseOver");
    }

    /// <summary>
    /// マウスがカードの上から離れた時
    /// </summary>
    void OnMouseExit()
    {
        IsMouseOver = false;
        //Debug.Log("OnMouseExit");
    }
}
    