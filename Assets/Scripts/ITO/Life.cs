using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    public int life_num = 3;
    [SerializeField] private Text textComponent;

    void Start()
    {
        textComponent.text = "Life" + life_num;
        
    }

    // çXêV
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            life_num -= 1;
        }
        textComponent.text = "Life" + life_num;
    }
}