using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChange : MonoBehaviour
{
    //�^�C�g���V�[������Q�[���V�[���ւ̑J��
    public void SwitchScene()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainGame");
    }

}
