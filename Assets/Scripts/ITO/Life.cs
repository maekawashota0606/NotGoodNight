using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{

    public GameObject life_object = null; // Text�I�u�W�F�N�g
    public int life_num = 3; // �X�R�A�ϐ�

    // ������
    void Start()
    {
    }

    // �X�V
    void Update()
    {
        // �I�u�W�F�N�g����Text�R���|�[�l���g���擾
        Text life_text = life_object.GetComponent<Text>();
        // �e�L�X�g�̕\�������ւ���
        life_text.text = "Life:" + life_num;
    }
}