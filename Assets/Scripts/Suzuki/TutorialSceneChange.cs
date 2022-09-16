using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TutorialSceneChange : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += LoopPointReached;
        videoPlayer.Play();
    }

    // Update is called once per frame
    void LoopPointReached(VideoPlayer vp)
    {
        SceneManager.LoadScene("MainGame");
    }
}
