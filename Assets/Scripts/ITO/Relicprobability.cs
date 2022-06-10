using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relicprobability : MonoBehaviour
{
    [SerializeField] private GameObject Prefab = null;
    [SerializeField] private Transform constant = null;
    [SerializeField] private Transform instant = null;
    public float n;
    int count1 = 0;
    int count2 = 0;
    bool isCalledOnce = false;
    void Start()
    {
        //n = Random.Range(0.0f, 10.0f);
        
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
    }
}
