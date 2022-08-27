using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ClickScript : MonoBehaviour
{
    private AudioSource button_AudioSource;
   
    // Use this for initialization
    void Start()
    {
        button_AudioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(button_AudioSource);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        button_AudioSource.PlayOneShot(button_AudioSource.clip);
    }
}
