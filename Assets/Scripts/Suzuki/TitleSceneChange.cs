using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChange : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(0);    
    }

    //タイトルシーンからゲームシーンへの遷移
    public void StartOnClick()
    {
        SoundManager.Instance.PlaySE(0);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("MainGame");
    }

    public void TutorialOnClick()
    {
        SoundManager.Instance.PlaySE(0);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Tutorial");
    }
}