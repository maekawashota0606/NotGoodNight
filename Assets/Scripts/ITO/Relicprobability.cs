using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relicprobability : MonoBehaviour
{
    /*[SerializeField] private GameObject Prefab = null;
    [SerializeField] private Transform constant = null;
    [SerializeField] private Transform instant = null;
    public float n;
    int count1 = 0;
    int count2 = 0;
    bool isCalledOnce = false;
    int start = 1;
    int end = 10;

    List<int> numbers = new List<int>();

    void Start()
    {
        //n = Random.Range(0.0f, 10.0f);
        for (int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isCalledOnce = false;
            // ランダムの範囲（今回は０から１０に設定）
            n = Random.Range(0.0f, 10.0f);
            // コンソール画面に数字を表示する。
            print(n);
        }
        if (!isCalledOnce)
        {
            if (n <= 0.15f && n != 0)
            {
                if(count1 <= 4)
                {
                    // プレハブを指定位置に生成
                    Instantiate(Prefab, constant);
                    isCalledOnce = true;
                    count1++;
                } 
            }
        }
        if (!isCalledOnce)
        {
            if (n <= 0.3f && n > 0.15f)
            {
                if (count2 <= 4)
                {
                    // プレハブを指定位置に生成
                    Instantiate(Prefab, instant);
                    isCalledOnce = true;
                    count2++;
                }
            }
        }
    }*/
    [SerializeField] private GameObject InstantPrefab1 = null;
    [SerializeField] private GameObject InstantPrefab2 = null;
    [SerializeField] private GameObject InstantPrefab3 = null;
    [SerializeField] private GameObject InstantPrefab4 = null;
    [SerializeField] private GameObject InstantPrefab5 = null;
    [SerializeField] private GameObject ConstantPrefab1 = null;
    [SerializeField] private GameObject ConstantPrefab2 = null;
    [SerializeField] private GameObject ConstantPrefab3 = null;
    [SerializeField] private GameObject ConstantPrefab4 = null;
    [SerializeField] private GameObject ConstantPrefab5 = null;
    [SerializeField] private Transform constant = null;
    [SerializeField] private Transform instant = null;

    int instantcount1 = 0;
    int instantcount2 = 0;
    int instantcount3 = 0;
    int instantcount4 = 0;
    int instantcount5 = 0;
    int constantcount1 = 0;
    int constantcount2 = 0;
    int constantcount3 = 0;
    int constantcount4 = 0;
    int constantcount5 = 0;

    // アイテムのデータを保持する辞書
    Dictionary<int, string> itemInfo;

    // 敵がドロップするアイテムの辞書
    Dictionary<int, float> itemDropDict;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetDropItem();
        }
    }

    void GetDropItem()
    {
        // 各種辞書の初期化
        InitializeDicts();

        // ドロップアイテムの抽選
        int itemId = Choose();

        // アイテムIDに応じたメッセージ出力
        if (itemId == 1 && instantcount1 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(InstantPrefab1, instant);
            instantcount1++;
        }
        else if (itemId == 2 && instantcount2 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(InstantPrefab2, instant);
            instantcount2++;
        }
        else if (itemId == 3 && instantcount3 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(InstantPrefab3, instant);
            instantcount3++;
        }
        else if (itemId == 4 && instantcount4 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(InstantPrefab4, instant);
            instantcount4++;
        }
        else if (itemId == 5 && instantcount5 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(InstantPrefab5, instant);
            instantcount5++;
        }
        else if (itemId == 6 && constantcount1 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(ConstantPrefab1, constant);
            constantcount1++;
        }
        else if (itemId == 7 && constantcount2 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(ConstantPrefab2, constant);
            constantcount2++;
        }
        else if (itemId == 8 && constantcount3 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(ConstantPrefab3, constant);
            constantcount3++;
        }
        else if (itemId == 9 && constantcount4 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(ConstantPrefab4, constant);
            constantcount4++;
        }
        else if (itemId == 10 && constantcount5 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " を入手した!");
            // プレハブを指定位置に生成
            Instantiate(ConstantPrefab5, constant);
            constantcount5++;
        }
        else
        {
            Debug.Log("アイテムは入手できませんでした。");
        }
    }

    void InitializeDicts()
    {
        itemInfo = new Dictionary<int, string>();
        itemInfo.Add(0, "なし");
        itemInfo.Add(1, "インスタント１");
        itemInfo.Add(2, "インスタント２");
        itemInfo.Add(3, "インスタント３");
        itemInfo.Add(4, "インスタント４");
        itemInfo.Add(5, "インスタント５");
        itemInfo.Add(6, "コンスタント１");
        itemInfo.Add(7, "コンスタント２");
        itemInfo.Add(8, "コンスタント３");
        itemInfo.Add(9, "コンスタント４");
        itemInfo.Add(10, "コンスタント５");

        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, 97.0f);
        itemDropDict.Add(1, 0.3f);
        itemDropDict.Add(2, 0.3f);
        itemDropDict.Add(3, 0.3f);
        itemDropDict.Add(4, 0.3f);
        itemDropDict.Add(5, 0.3f);
        itemDropDict.Add(6, 0.3f);
        itemDropDict.Add(7, 0.3f);
        itemDropDict.Add(8, 0.3f);
        itemDropDict.Add(9, 0.3f);
        itemDropDict.Add(10, 0.3f);
    }
    
    int Choose()
    {
        // 確率の合計値を格納
        float total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        float randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }
}
