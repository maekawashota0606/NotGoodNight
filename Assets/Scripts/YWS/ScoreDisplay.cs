using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private Text scoreText = null;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score : " + Player.Score.ToString("d6");
    }
}
