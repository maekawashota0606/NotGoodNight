using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText1 : MonoBehaviour
{
    int matumura = 0;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (matumura)
        {
            case 0:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "隕石を2×2マスに纏める";
                break;
            case 1:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "ランダムに選択された9個の隕石を3×3マスに纏める";
                break;
            case 2:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "得意遺物をランダムに1つ入手する";
                break;
            case 3:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "ライフを1回復する";
                break;
            case 4:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "手札のカードを一枚選択し、コピーを作る";
                break;
            case 5:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "二枚分のコストになる";
                break;
            case 6:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "このカードをコスト支払いに使用したとき、隕石が下がらない";
                break;
            case 7:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "コスト支払いに使用したとき、ランダムな隕石を二つ破壊する";
                break;
            case 8:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "最上段一列に隕石を10個生成　カードを6枚引く";
                break;
            case 9:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "隕石を十字に生成する・カードを3枚引く";
                break;
            case 10:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "このカード使用後を含み3回、隕石の落下を止める";
                break;
            case 11:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2種類のカードをランダムに手札に加え、コストを0にする";
                break;
            case 12:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "3ターン無敵になる";
                break;
            case 13:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "カードを3枚引く";
                break;
            case 14:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "前ターンに破壊された隕石の数だけカードを引く";
                break;
            case 15:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "手札がこのカードのみなら、7枚引く";
                break;
            case 16:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "カードをを3枚引き、それぞれコストを1下げる";
                break;
            case 17:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2枚引く　30000ごとに追加2ドロー";
                break;
            case 18:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "縦4マスの隕石を破壊する";
                break;
            case 19:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "横4マスの隕石を破壊する";
                break;
            case 20:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "上下左右４マスの隕石を破壊する";
                break;
            case 21:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2×2マスを破壊する";
                break;
            case 22:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "横一列の隕石を破壊する";
                break;
            case 23:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "全ての隣接している隕石を破壊する";
                break;
            case 24:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8マス破壊する";
                break;
            case 25:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8マス破壊する";
                break;
            case 26:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8マス破壊する";
                break;
            case 27:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8マス破壊する";
                break;
            case 28:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "3×3マスの隕石を破壊、カードを3枚ドロー";
                break;
            case 29:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "隕石を引き寄せ、消滅させる";
                break;
            case 30:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "ランダムに50マス選択し、隕石を破壊する";
                break;
            case 31:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "横四マス破壊する　　上から5列の中に隕石をランダムに4つ生成する";
                break;
            case 32:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "破壊した隕石の数カードをひく";
                break;
            case 33:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "5×５破壊　30000点を超えていたら５ドロー";
                break;
            case 34:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "下から4列にある隕石を5列目まで押し上げる";
                break;
        }
    }
}

