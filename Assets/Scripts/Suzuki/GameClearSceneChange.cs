using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneChange : MonoBehaviour
{
    //ゲームクリアシーンからタイトルシーンへの遷移
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
