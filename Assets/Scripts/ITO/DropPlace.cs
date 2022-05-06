using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// �t�B�[���h�ɃA�^�b�`����N���X
public class DropPlace : MonoBehaviour, IDropHandler
{
    // �h���b�v���ꂽ���ɍs������
    public void OnDrop(PointerEventData eventData) 
    {
        // �h���b�O���Ă�����񂩂�CardMovement���擾
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // �����J�[�h������΁A
        if (card != null) 
        {
            // �J�[�h�̐e�v�f�������i�A�^�b�`����Ă�I�u�W�F�N�g�j�ɂ���
            card.cardParent = this.transform; 
        }
    }
}