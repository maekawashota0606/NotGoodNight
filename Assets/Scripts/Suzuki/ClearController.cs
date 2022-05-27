using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearController : MonoBehaviour
{
    //private float step_time;

    // Start is called before the first frame update
    void Start()
    {
        //step_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchScene();
        }

        //step_time += Time.deltaTime;
        //if (step_time >= 3.0f)
        //{
        //    SceneManager.LoadScene("Title");
        //}
    }

    public void SwitchScene()
    {
        
        SceneManager.LoadScene("Title");
    }
}
