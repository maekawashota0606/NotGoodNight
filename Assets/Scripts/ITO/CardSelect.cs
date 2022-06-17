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
    GameObject aaa;
    Count script;
    public Text TextFrame;

    void Start()
    {
        aaa = GameObject.Find("CountManager");
        script = aaa.GetComponent<Count>();
    }

    void Update()
    {
        int count = script.counta;
        //選択されていないカードをクリックしたら、そのカードの枠を赤色にする
        if (IsMouseOver == true && Input.GetMouseButtonDown(0))
        {
            
            if (count == 0)
            {
                image_component.color = Color.red;
                IsClick = true;
                GameDirector.Instance.IsCardSelect = true;
                Debug.Log(this.name + "Selected");
                script.counta += 1;
                
            }
            else
            {
                image_component.color = Color.green;
                IsClick = true;
                GameDirector.Instance.IsCardSelect = true;
                Debug.Log(this.name + "Selected");
                
            }
        }
        

        //選択されたカードを右クリックしたら、そのカードの選択を解除し、枠を白色にする
        if (IsMouseOver == true && Input.GetMouseButtonDown(1))
        {
            image_component.color = Color.white;
            IsClick = false;
            GameDirector.Instance.IsCardSelect = false;
            script.counta = 0;
        }

        //このカードが選択されたいない場合
        if (IsClick == false)
        {
            //マウスが乗っていたら、カードの枠を黄色にする
            if (IsMouseOver == true)
            {
                image_component.color = Color.yellow;
                TextFrame.text = string.Format("frame");
            }
            //乗っていない場合、白色にする
            else
            {
                image_component.color = Color.white;
            }
        }

        if (IsClick == true && GameDirector.Instance.IsCardUsed == true)
        {
            Destroy(this);
            GameDirector.Instance.IsCardUsed = false;
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