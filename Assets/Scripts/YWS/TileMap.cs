using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : SingletonMonoBehaviour<TileMap>
{
    public GameObject[,] map = new GameObject[10, 10];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameDirector.Instance.NeedSearch == true)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (map[j, i].tag == "Selected")
                    {
                        Debug.Log("hit" + map[j, i].name);
                        GameDirector.Instance.MeteorDestory(j, i);
                        map[j, i].tag = "Untagged";
                        break;
                    }
                }
            }
        }
    }
}