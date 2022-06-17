using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relicprobability : MonoBehaviour
{
    /*[SerializeField] private GameObject Prefab = null;
    [SerializeField] private Transform constant = null;
    [SerializeField] private Transform instant = null;
    public float n;
    int count1 = 0;
    int count2 = 0;
    bool isCalledOnce = false;
    int start = 1;
    int end = 10;

    List<int> numbers = new List<int>();

    void Start()
    {
        //n = Random.Range(0.0f, 10.0f);
        for (int i = start; i <= end; i++)
        {
            numbers.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isCalledOnce = false;
            // �����_���͈̔́i����͂O����P�O�ɐݒ�j
            n = Random.Range(0.0f, 10.0f);
            // �R���\�[����ʂɐ�����\������B
            print(n);
        }
        if (!isCalledOnce)
        {
            if (n <= 0.15f && n != 0)
            {
                if(count1 <= 4)
                {
                    // �v���n�u���w��ʒu�ɐ���
                    Instantiate(Prefab, constant);
                    isCalledOnce = true;
                    count1++;
                } 
            }
        }
        if (!isCalledOnce)
        {
            if (n <= 0.3f && n > 0.15f)
            {
                if (count2 <= 4)
                {
                    // �v���n�u���w��ʒu�ɐ���
                    Instantiate(Prefab, instant);
                    isCalledOnce = true;
                    count2++;
                }
            }
        }
    }*/
    [SerializeField] private GameObject InstantPrefab1 = null;
    [SerializeField] private GameObject InstantPrefab2 = null;
    [SerializeField] private GameObject InstantPrefab3 = null;
    [SerializeField] private GameObject InstantPrefab4 = null;
    [SerializeField] private GameObject InstantPrefab5 = null;
    [SerializeField] private GameObject ConstantPrefab1 = null;
    [SerializeField] private GameObject ConstantPrefab2 = null;
    [SerializeField] private GameObject ConstantPrefab3 = null;
    [SerializeField] private GameObject ConstantPrefab4 = null;
    [SerializeField] private GameObject ConstantPrefab5 = null;
    [SerializeField] private Transform constant = null;
    [SerializeField] private Transform instant = null;

    int instantcount1 = 0;
    int instantcount2 = 0;
    int instantcount3 = 0;
    int instantcount4 = 0;
    int instantcount5 = 0;
    int constantcount1 = 0;
    int constantcount2 = 0;
    int constantcount3 = 0;
    int constantcount4 = 0;
    int constantcount5 = 0;

    // �A�C�e���̃f�[�^��ێ����鎫��
    Dictionary<int, string> itemInfo;

    // �G���h���b�v����A�C�e���̎���
    Dictionary<int, float> itemDropDict;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetDropItem();
        }
    }

    void GetDropItem()
    {
        // �e�펫���̏�����
        InitializeDicts();

        // �h���b�v�A�C�e���̒��I
        int itemId = Choose();

        // �A�C�e��ID�ɉ��������b�Z�[�W�o��
        if (itemId == 1 && instantcount1 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(InstantPrefab1, instant);
            instantcount1++;
        }
        else if (itemId == 2 && instantcount2 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(InstantPrefab2, instant);
            instantcount2++;
        }
        else if (itemId == 3 && instantcount3 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(InstantPrefab3, instant);
            instantcount3++;
        }
        else if (itemId == 4 && instantcount4 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(InstantPrefab4, instant);
            instantcount4++;
        }
        else if (itemId == 5 && instantcount5 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(InstantPrefab5, instant);
            instantcount5++;
        }
        else if (itemId == 6 && constantcount1 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(ConstantPrefab1, constant);
            constantcount1++;
        }
        else if (itemId == 7 && constantcount2 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(ConstantPrefab2, constant);
            constantcount2++;
        }
        else if (itemId == 8 && constantcount3 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(ConstantPrefab3, constant);
            constantcount3++;
        }
        else if (itemId == 9 && constantcount4 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(ConstantPrefab4, constant);
            constantcount4++;
        }
        else if (itemId == 10 && constantcount5 == 0)
        {
            string itemName = itemInfo[itemId];
            Debug.Log(itemName + " ����肵��!");
            // �v���n�u���w��ʒu�ɐ���
            Instantiate(ConstantPrefab5, constant);
            constantcount5++;
        }
        else
        {
            Debug.Log("�A�C�e���͓���ł��܂���ł����B");
        }
    }

    void InitializeDicts()
    {
        itemInfo = new Dictionary<int, string>();
        itemInfo.Add(0, "�Ȃ�");
        itemInfo.Add(1, "�C���X�^���g�P");
        itemInfo.Add(2, "�C���X�^���g�Q");
        itemInfo.Add(3, "�C���X�^���g�R");
        itemInfo.Add(4, "�C���X�^���g�S");
        itemInfo.Add(5, "�C���X�^���g�T");
        itemInfo.Add(6, "�R���X�^���g�P");
        itemInfo.Add(7, "�R���X�^���g�Q");
        itemInfo.Add(8, "�R���X�^���g�R");
        itemInfo.Add(9, "�R���X�^���g�S");
        itemInfo.Add(10, "�R���X�^���g�T");

        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, 97.0f);
        itemDropDict.Add(1, 0.3f);
        itemDropDict.Add(2, 0.3f);
        itemDropDict.Add(3, 0.3f);
        itemDropDict.Add(4, 0.3f);
        itemDropDict.Add(5, 0.3f);
        itemDropDict.Add(6, 0.3f);
        itemDropDict.Add(7, 0.3f);
        itemDropDict.Add(8, 0.3f);
        itemDropDict.Add(9, 0.3f);
        itemDropDict.Add(10, 0.3f);
    }
    
    int Choose()
    {
        // �m���̍��v�l���i�[
        float total = 0;

        // �G�h���b�v�p�̎�������h���b�v�������v����
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.value�ł�0����1�܂ł�float�l��Ԃ��̂�
        // �����Ƀh���b�v���̍��v���|����
        float randomPoint = Random.value * total;

        // randomPoint�̈ʒu�ɊY������L�[��Ԃ�
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }
}
