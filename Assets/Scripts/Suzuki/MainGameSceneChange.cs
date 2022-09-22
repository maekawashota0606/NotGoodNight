using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameSceneChange : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SceneManager.LoadScene("Title");
        }

        if (GameDirector.Instance.gameState == GameDirector.GameState.ended && GameDirector.Instance.IsPlayerWin == true)
        {
            SwitchScene_GameClear();
        }
        else if (GameDirector.Instance.gameState == GameDirector.GameState.ended && GameDirector.Instance.IsPlayerWin == false)
        {
            SwitchScene_GameOver();
        }
    }

    //�Q�[���N���A�V�[���ւ̑J��
    private void SwitchScene_GameClear()
    {
        SceneManager.LoadScene("Gameclear");
    }

    //�Q�[���I�[�o�[�V�[���ւ̑J��
    private void SwitchScene_GameOver()
    {
        SceneManager.LoadScene("Gameover");
    }
}
