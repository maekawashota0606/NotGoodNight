using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonoBehaviour<Map>
{
    public string[,] map = new string[11, 10] // z, x座標で指定
    {
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"x", "x", "x", "x", "x", "x", "x", "x", "x", "x"},
    };

    public readonly string field = "□";
    public readonly string deleteZone = "x";

    // map確認用
    public void CheckMap()
    {
        string s = "";
        for (int a = 0; a < 10; a++)
        {
            for (int b = 0; b < 10; b++)
            {
                s += map[a, b];
            }
            s += "\n";
        }
        Debug.Log(s);
    }
}