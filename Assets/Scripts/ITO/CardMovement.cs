using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    // �h���b�O���n�߂�Ƃ��ɍs������
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        // blocksRaycasts���I�t�ɂ���
        GetComponent<CanvasGroup>().blocksRaycasts = false; 
    }
    // �h���b�O�������ɋN��������
    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = eventData.position;
    }
    // �J�[�h�𗣂����Ƃ��ɍs������
    public void OnEndDrag(PointerEventData eventData) 
    {
        transform.SetParent(cardParent, false);
    }
}


