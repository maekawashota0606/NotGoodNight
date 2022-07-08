using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneChange : MonoBehaviour
{
    //���g���C
    public void SwitchScene_MainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    //�^�C�g���V�[���ւ̑J��
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
