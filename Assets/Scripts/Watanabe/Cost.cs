//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

/*public class Cost : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text playerManaPointText;
    [SerializeField] Text playerDefaultManaPointText;

    void StartGame() // �����l�̐ݒ� 
    {
        /// �}�i�̏����l�ݒ� ///
        playerManaPoint = 1;
        playerDefaultManaPoint = 1;
        ShowManaPoint();
    }
    void ShowManaPoint() // �}�i�|�C���g��\�����郁�\�b�h
    {
        playerManaPointText.text = playerManaPoint.ToString();
        playerDefaultManaPointText.text = playerDefaultManaPoint.ToString();
    }
}*/







/*public class CardMovement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform cardParent;
    bool canDrag = true;

    public void OnBeginDrag(PointerEventData eventData) // �h���b�O���n�߂�Ƃ��ɍs������
    {

        CardController card = GetComponent();

        canDrag = true;

        if (card.model.FieldCard == false) // ��D�̃J�[�h�Ȃ�
        {
            if (card.model.canUse == false) // �}�i�R�X�g��菭�Ȃ��J�[�h�͓������Ȃ�
            {
                canDrag = false;
            }
        }
        // Update is called once per frame
        void Update()
    {
        
    }
}*/
