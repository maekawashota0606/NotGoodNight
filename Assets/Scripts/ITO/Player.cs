using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //カードプレハブ
    [SerializeField] GameObject cardPrefab;
    //手札置き場
    [SerializeField] Transform playerHand;
    //スコア
    public int Score = 0;
    //ライフ
    public int Life = 3;

    public void PlayerControl()
    {

    }

    public void DrawCard()
    {
        if (GameDirector.Instance.gameState == GameDirector.GameState.active && GameDirector.Instance.CanPlayerControl == true)
        {
            Instantiate(cardPrefab, playerHand);
            GameDirector.Instance.CanPlayerControl = false;
            GameDirector.Instance.IsPlayerSelectMove = true;
        }
        else if (GameDirector.Instance.gameState == GameDirector.GameState.standby)
        {
            Instantiate(cardPrefab, playerHand);
        }
    }
}
