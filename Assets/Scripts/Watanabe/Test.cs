using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            AudioManager.Instance.PlayBGM("¯‹ó");

        }
        if (Input.GetKey(KeyCode.I))
        {
            AudioManager.Instance.PlaySE("‘I‘ğ");
        }
    }
}
