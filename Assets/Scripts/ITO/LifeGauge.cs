using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeGauge : MonoBehaviour
{
    // �]��񕜕�
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject obj5;
    public GameObject obj6;
    public GameObject obj7;
    public GameObject obj8;
    public GameObject obj9;
    // �����ɕK�v
    private Image image1;
    private Image image2;
    private Image image3;
    // �̗͐Ԃ̉摜
    public Sprite ime1;
    public Sprite ime2;
    public Sprite ime3;
    // �̗͍��̉摜
    public Sprite ime11;
    public Sprite ime22;
    public Sprite ime33;
    // switch��
    public int a = 3;
    private void Start()
    {
        // ����
        image1 = GameObject.Find("Image").GetComponent<Image>();
        image2 = GameObject.Find("Image1").GetComponent<Image>();
        image3 = GameObject.Find("Image2").GetComponent<Image>();
    }
    private void Update()
    {
        // ��
        if (Input.GetKeyDown(KeyCode.L))
        {
            a++;
        }
        // �_���[�W
        if (Input.GetKeyDown(KeyCode.K))
        {
            a--;
        }

        switch (a)
        {
            case 0:
                image1.sprite = ime11;
                image2.sprite = ime22;
                image3.sprite = ime33;
                break;
            case 1:
                image1.sprite = ime11;
                image2.sprite = ime22;
                image3.sprite = ime3;
                break;
            case 2:
                image1.sprite = ime11;
                image2.sprite = ime2;
                
                break;
            case 3:
                image1.sprite = ime1;
                obj1.SetActive(false);
                break;
            case 4:
                obj1.SetActive(true);
                obj2.SetActive(false);
                break;
            case 5:
                obj2.SetActive(true);
                obj3.SetActive(false);
                break;
            case 6:
                obj3.SetActive(true);
                obj4.SetActive(false);
                break;
            case 7:
                obj4.SetActive(true);
                obj5.SetActive(false);
                break;
            case 8:
                obj5.SetActive(true);
                obj6.SetActive(false);
                break;
            case 9:
                obj6.SetActive(true);
                obj7.SetActive(false);
                break;
            case 10:
                obj7.SetActive(true);
                obj8.SetActive(false);
                break;
            case 11:
                obj8.SetActive(true);
                obj9.SetActive(false);
                break;
            case 12:
                obj9.SetActive(true);
                break;
        }
        // �񕜏��
        if (a == 13)
        {
            a = 12;
        }
        // �Q�[���I�[�o�[����
        if (a <= 0)
        {
            a = 0;
        }
    }
}
