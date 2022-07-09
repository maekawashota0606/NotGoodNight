using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneChange : MonoBehaviour
{
    //リトライ
    public void SwitchScene_MainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    //タイトルシーンへの遷移
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
