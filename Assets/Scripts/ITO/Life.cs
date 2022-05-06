using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{

    public GameObject life_object = null; // Textオブジェクト
    public int life_num = 3; // スコア変数

    // 初期化
    void Start()
    {
    }

    // 更新
    void Update()
    {
        // オブジェクトからTextコンポーネントを取得
        Text life_text = life_object.GetComponent<Text>();
        // テキストの表示を入れ替える
        life_text.text = "Life:" + life_num;
    }
}