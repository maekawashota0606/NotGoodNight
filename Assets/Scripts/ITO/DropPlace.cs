using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// フィールドにアタッチするクラス
public class DropPlace : MonoBehaviour, IDropHandler
{
    // ドロップされた時に行う処理
    public void OnDrop(PointerEventData eventData) 
    {
        // ドラッグしてきた情報からCardMovementを取得
        CardMovement card = eventData.pointerDrag.GetComponent<CardMovement>();
        // もしカードがあれば、
        if (card != null) 
        {
            // カードの親要素を自分（アタッチされてるオブジェクト）にする
            card.cardParent = this.transform; 
        }
    }
}