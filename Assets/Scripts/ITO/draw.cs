using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand;

    public void OnClick()
    {
        Drawat();
    }

    public void Drawat()
    {
        Instantiate(cardPrefab, playerHand);
    }
}
