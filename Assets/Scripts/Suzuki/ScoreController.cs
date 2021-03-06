using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = "Score" + score.ToString("d6");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddScore();
        }
    }

    public void AddScore()
    {
        score += 10000;
        textComponent.text = "Score" + score.ToString("d6");
    }
}
