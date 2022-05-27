using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draw : MonoBehaviour
{
    [SerializeField] GameObject cardPrefab;
    [SerializeField] Transform playerHand;

    public void OnClick()
    {
        Draw();
    }

    public void Draw()
    {
        Instantiate(cardPrefab, playerHand);
    }

}