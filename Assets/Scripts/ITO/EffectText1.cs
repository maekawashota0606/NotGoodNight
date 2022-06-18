using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText1 : MonoBehaviour
{
    [SerializeField]
    Image image_component = null;
    public Text TextFrame;
    public bool IsMouseOver = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMouseOver == true)
        {
            TextFrame.text = string.Format("frame");
        }
        
    }
}
