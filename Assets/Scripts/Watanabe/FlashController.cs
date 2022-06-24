using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FlashController : MonoBehaviour
{
    private Image img;
    public float color;
    public Color clear;
    public float deltaTime;
    void Start()
    {
        img = GetComponent<Image>();
        img.color = Color.clear;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            img.color = new Color(1,1,1,1);
        }
        else
        {
            img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime);
        }
    }
}