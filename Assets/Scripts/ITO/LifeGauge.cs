using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{
    [SerializeField, Header("ライフゲージ")] private GameObject[] LifeObj = new GameObject[6];
    private List<SpriteRenderer> LifeGaugeList = new List<SpriteRenderer>();
    [SerializeField, Header("ライフゲージ画像")] private Sprite[] LifeImage = new Sprite[6];

    private void Start()
    {
        for (int i = 0; i < LifeObj.Length; i++)
        {
            LifeGaugeList.Add(LifeObj[i].GetComponent<SpriteRenderer>());
        }
    }

    private void Update()
    {
        switch (GameDirector.Instance._player.Life)
        {
            case 0:
                LifeGaugeList[0].sprite = LifeImage[3];
                LifeGaugeList[1].sprite = LifeImage[4];
                LifeGaugeList[2].sprite = LifeImage[5];
                LifeObj[3].SetActive(false);
                LifeObj[4].SetActive(false);
                LifeObj[5].SetActive(false);
                break;
            case 1:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[4];
                LifeGaugeList[2].sprite = LifeImage[5];
                LifeObj[3].SetActive(false);
                LifeObj[4].SetActive(false);
                LifeObj[5].SetActive(false);
                break;
            case 2:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[1];
                LifeGaugeList[2].sprite = LifeImage[5];
                LifeObj[3].SetActive(false);
                LifeObj[4].SetActive(false);
                LifeObj[5].SetActive(false);
                break;
            case 3:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[1];
                LifeGaugeList[2].sprite = LifeImage[2];
                LifeObj[3].SetActive(false);
                LifeObj[4].SetActive(false);
                LifeObj[5].SetActive(false);
                break;
            case 4:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[1];
                LifeGaugeList[2].sprite = LifeImage[2];
                LifeObj[3].SetActive(true);
                LifeObj[4].SetActive(false);
                LifeObj[5].SetActive(false);
                break;
            case 5:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[1];
                LifeGaugeList[2].sprite = LifeImage[2];
                LifeObj[3].SetActive(true);
                LifeObj[4].SetActive(true);
                LifeObj[5].SetActive(false);
                break;
            case 6:
                LifeGaugeList[0].sprite = LifeImage[0];
                LifeGaugeList[1].sprite = LifeImage[1];
                LifeGaugeList[2].sprite = LifeImage[2];
                LifeObj[3].SetActive(true);
                LifeObj[4].SetActive(true);
                LifeObj[5].SetActive(true);
                break;

            default:
                break;
        }
    }
}
