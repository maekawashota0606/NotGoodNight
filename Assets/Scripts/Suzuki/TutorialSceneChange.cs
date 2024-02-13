using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TutorialSceneChange : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer = null;
    private bool repeatHit = false;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (repeatHit)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            repeatHit = true;

            SwitchScene_Title();
        }
    }

    private void LoopPointReached(VideoPlayer vp)
    {
        repeatHit = true;
        SwitchScene_Title();
    }

    private void SwitchScene_Title()
    {
        SceneManager.LoadScene("Title");
    }
}
