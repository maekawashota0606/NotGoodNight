using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand;

    void Start()
    {
        // ��D��z��i�����j
        for (int i = 0; i < 5; i++)
        {
            Instantiate(cardPrefab, playerHand);
        }
    }
}
