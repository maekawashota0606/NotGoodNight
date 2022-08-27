using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*BGM*/
        ////////////////////////////////////////
        if (Input.GetKey(KeyCode.L))
        {
            AudioManager.Instance.PlayBGM("星空");//サンプル

        }
        /*SE*/
        ////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySE("Se_0001");//タイトル決定
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlaySE("Se_0002");//カーソル音
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioManager.Instance.PlaySE("Se_0003");//カード決定音
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioManager.Instance.PlaySE("Se_0004");//使用カード決定音
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            AudioManager.Instance.PlaySE("Se_0005");//範囲カーソル決定音
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AudioManager.Instance.PlaySE("Se_0006");//効果実行音
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AudioManager.Instance.PlaySE("Se_0007");//隕石爆発音
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.PlaySE("Se_0008");//カード補充音
        }

    }
}
