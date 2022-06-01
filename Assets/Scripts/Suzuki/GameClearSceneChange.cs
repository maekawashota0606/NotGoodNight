using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearSceneChange : MonoBehaviour
{
    public void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
