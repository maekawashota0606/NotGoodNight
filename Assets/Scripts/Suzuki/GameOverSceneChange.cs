using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneChange : MonoBehaviour
{
    public void SwitchScene_MainGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
