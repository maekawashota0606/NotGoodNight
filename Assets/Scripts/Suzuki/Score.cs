using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int score = 0;
    [SerializeField]private Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = "Score" + score.ToString("d7");
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
        score += 1000;
        textComponent.text = "Score" + score.ToString("d7");
    }
}
