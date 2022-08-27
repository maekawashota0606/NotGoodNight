using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneChange : MonoBehaviour
{
    //タイトルシーンからゲームシーンへの遷移
    public void SwitchScene()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainGame");
    }

}
