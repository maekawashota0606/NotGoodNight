using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Map.Instance.CheckMap();
        Generate();
        Map.Instance.CheckMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MeteorMove();
        }
    }

    private void Generate()
    {
        Map.Instance.map[0,5] = "x";
    }

    public void MeteorMove()
    {

    }
}
