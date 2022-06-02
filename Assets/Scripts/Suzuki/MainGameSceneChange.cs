using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    void Update()
    {
        if (GameDirector.Instance.gameState == GameDirector.GameState.ended && GameDirector.Instance.IsPlayerWin == true)
        {
            SwitchScene_GameClear();
        }
        else if (GameDirector.Instance.gameState == GameDirector.GameState.ended && GameDirector.Instance.IsPlayerWin == false)
        {
            SwitchScene_GameOver();
        }
    }

    public void SwitchScene_GameClear()
    {
        SceneManager.LoadScene("Gameclear");
    }

    public void SwitchScene_GameOver()
    {
        SceneManager.LoadScene("Gameover");
    }
}
