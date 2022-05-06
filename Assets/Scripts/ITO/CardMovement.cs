using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    // ドラッグを始めるときに行う処理
    public void OnBeginDrag(PointerEventData eventData)
    {
        cardParent = transform.parent;
        transform.SetParent(cardParent.parent, false);
        // blocksRaycastsをオフにする
        GetComponent<CanvasGroup>().blocksRaycasts = false; 
    }
    // ドラッグした時に起こす処理
    public void OnDrag(PointerEventData eventData) 
    {
        transform.position = eventData.position;
    }
    // カードを離したときに行う処理
    public void OnEndDrag(PointerEventData eventData) 
    {
        transform.SetParent(cardParent, false);
    }
}


