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
            AudioManager.Instance.PlayBGM("����");//�T���v��

        }
        /*SE*/
        ////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlaySE("Se_0001");//�^�C�g������
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlaySE("Se_0002");//�J�[�\����
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            AudioManager.Instance.PlaySE("Se_0003");//�J�[�h���艹
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AudioManager.Instance.PlaySE("Se_0004");//�g�p�J�[�h���艹
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            AudioManager.Instance.PlaySE("Se_0005");//�͈̓J�[�\�����艹
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            AudioManager.Instance.PlaySE("Se_0006");//���ʎ��s��
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AudioManager.Instance.PlaySE("Se_0007");//覐Δ�����
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.PlaySE("Se_0008");//�J�[�h��[��
        }

    }
}
