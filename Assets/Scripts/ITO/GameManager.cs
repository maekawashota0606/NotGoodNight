using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand;

    void Start()
    {
        // èD‚ğ”z‚éi©•ªj
        for (int i = 0; i < 3; i++)
        {
            Instantiate(cardPrefab, playerHand);
        }
    }
}
