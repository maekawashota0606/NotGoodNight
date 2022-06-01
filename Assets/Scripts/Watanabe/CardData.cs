using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    //���
    public enum CardType
    {
        Attack,     //�U���i�j������j
        Defence    //�h��i��������j
    }
    //����
    public enum CardEffective
    {
        Destroy,    //�j��
        Draw,       //�h���[
        Move,       //�ړ�
        Stop,        //���
        Dump,      //�̂Ă�
        Create,     //����
        Heel,        //�� 
        Trump,     //�؂�D
        Border,     //�{�[�_�[
        Up,          // �����グ�i覐΂���ɂ�����j
        Copy,       //�R�s�[
        Reduse,   //�R�X�g�y��
        Extra,      //�O������
    }
    public int ID = 0;
    public int Cost = 1;
    public CardType CardTypeValue = CardData.CardType.Attack;
    public CardEffective CardEffectiveValue = CardData.CardEffective.Destroy;

    public void Init(int id,int cost,CardType type,CardEffective effective)
    {
        this.ID = id;
        this.Cost = cost;
        this.CardTypeValue = type;
        this.CardEffectiveValue = effective;
    }
}


