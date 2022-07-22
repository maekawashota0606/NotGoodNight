using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneChange : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(2);    
    }

    //�Q�[���N���A�V�[������^�C�g���V�[���ւ̑J��
    public void SwitchScene_Title()
    {
        SoundManager.Instance.PlaySE(0);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Title");
    }
}
