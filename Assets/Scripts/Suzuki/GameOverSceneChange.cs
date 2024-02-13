using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneChange : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM(3);    
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SwitchScene_Title();
        }
    }

    //タイトルシーンへの遷移
    private void SwitchScene_Title()
    {
        SoundManager.Instance.PlaySE(0);
        SoundManager.Instance.StopBGM();
        SceneManager.LoadScene("Title");
    }
}
