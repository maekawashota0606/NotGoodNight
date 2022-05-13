//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

/*public class Cost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text playerManaPointText;
    [SerializeField] Text playerDefaultManaPointText;

    void StartGame() // 初期値の設定 
    {
        /// マナの初期値設定 ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();
    }
    void ShowManaPoint() // マナポイントを表示するメソッド
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }
}*/







/*public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    bool canDrag = true;

    public void OnBeginDrag(PointerEventData eventData) // ドラッグを始めるときに行う処理
    {

        CardController card = GetComponent();

        canDrag = true;

        if (card.model.FieldCard == false) // 手札のカードなら
        {
            if (card.model.canUse == false) // マナコストより少ないカードは動かせない
            {
                canDrag = false;
            }
        }
        // Update is called once per frame
        void Update()
    {
        
    }
}*/
