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
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "覐΂�2�~2�}�X�ɓZ�߂�";
                break;
            case 1:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�����_���ɑI�����ꂽ9��覐΂�3�~3�}�X�ɓZ�߂�";
                break;
            case 2:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "���ӈ╨�������_����1���肷��";
                break;
            case 3:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "���C�t��1�񕜂���";
                break;
            case 4:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "��D�̃J�[�h���ꖇ�I�����A�R�s�[�����";
                break;
            case 5:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�񖇕��̃R�X�g�ɂȂ�";
                break;
            case 6:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "���̃J�[�h���R�X�g�x�����Ɏg�p�����Ƃ��A覐΂�������Ȃ�";
                break;
            case 7:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�R�X�g�x�����Ɏg�p�����Ƃ��A�����_����覐΂��j�󂷂�";
                break;
            case 8:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�ŏ�i����覐΂�10�����@�J�[�h��6������";
                break;
            case 9:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "覐΂��\���ɐ�������E�J�[�h��3������";
                break;
            case 10:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "���̃J�[�h�g�p����܂�3��A覐΂̗������~�߂�";
                break;
            case 11:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2��ނ̃J�[�h�������_���Ɏ�D�ɉ����A�R�X�g��0�ɂ���";
                break;
            case 12:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "3�^�[�����G�ɂȂ�";
                break;
            case 13:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�J�[�h��3������";
                break;
            case 14:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�O�^�[���ɔj�󂳂ꂽ覐΂̐������J�[�h������";
                break;
            case 15:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "��D�����̃J�[�h�݂̂Ȃ�A7������";
                break;
            case 16:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�J�[�h����3�������A���ꂼ��R�X�g��1������";
                break;
            case 17:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2�������@30000���Ƃɒǉ�2�h���[";
                break;
            case 18:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�c4�}�X��覐΂�j�󂷂�";
                break;
            case 19:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "��4�}�X��覐΂�j�󂷂�";
                break;
            case 20:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�㉺���E�S�}�X��覐΂�j�󂷂�";
                break;
            case 21:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "2�~2�}�X��j�󂷂�";
                break;
            case 22:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "������覐΂�j�󂷂�";
                break;
            case 23:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�S�Ă̗אڂ��Ă���覐΂�j�󂷂�";
                break;
            case 24:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8�}�X�j�󂷂�";
                break;
            case 25:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8�}�X�j�󂷂�";
                break;
            case 26:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8�}�X�j�󂷂�";
                break;
            case 27:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "8�}�X�j�󂷂�";
                break;
            case 28:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "3�~3�}�X��覐΂�j��A�J�[�h��3���h���[";
                break;
            case 29:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "覐΂������񂹁A���ł�����";
                break;
            case 30:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�����_����50�}�X�I�����A覐΂�j�󂷂�";
                break;
            case 31:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "���l�}�X�j�󂷂�@�@�ォ��5��̒���覐΂������_����4��������";
                break;
            case 32:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "�j�󂵂�覐΂̐��J�[�h���Ђ�";
                break;
            case 33:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "5�~�T�j��@30000�_�𒴂��Ă�����T�h���[";
                break;
            case 34:
                gameObject.GetComponent<UnityEngine.UI.Text>().text = "������4��ɂ���覐΂�5��ڂ܂ŉ����グ��";
                break;
        }
    }
}

