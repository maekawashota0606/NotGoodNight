using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneChange : MonoBehaviour
{
    //�Q�[���N���A�V�[������^�C�g���V�[���ւ̑J��
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
