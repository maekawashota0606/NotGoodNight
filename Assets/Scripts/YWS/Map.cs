using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonoBehaviour<Map>
{
    //ステージの大きさ
    private static int width = 10;
    private static int height = 10;

    //マップ
    private static Transform[,] grid = new Transform[width,height];
}