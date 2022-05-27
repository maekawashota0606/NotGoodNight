using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public ScoreController scoreController;
    public Life life;

    void Start()
    {
        
    }

    void Update()
    {
        int scoremng = scoreController.score;
        if (scoremng > 99999)
        {
            SwitchScene_gameclear();
        }

        int lifemng = life.life_num;
        if (lifemng == 0)
        {
            SwitchScene_gameover();
        }
    }

    public void SwitchScene_gameclear()
    {
        SceneManager.LoadScene("Gameclear", LoadSceneMode.Single);
    }

    public void SwitchScene_gameover()
    {
        SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
    }
}
