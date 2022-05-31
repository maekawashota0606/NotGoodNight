using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonoBehaviour<Map>
{
    //ステージの大きさ
    private const int width = 10;
    private const int height = 10;

    //マップ
    public string[,] map = new string[height, width] // z, x座標で指定
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
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"}
    };

    public readonly string empty = "□";
    public readonly string meteor = "●";

    /// <summary>
    /// 生成する場所にすでに隕石が存在しているかどうか
    /// </summary>
    /// <param name="searchPos">検索する座標</param>
    /// <returns></returns>
    public bool CheckEmpty(Vector3 searchPos)
    {
        int x = (int)searchPos.x;
        int z = (int)searchPos.z * -1;

        if (map[z, x] == meteor)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    /// <summary>
    /// 隕石の下１マスは空白かどうか
    /// </summary>
    /// <param name="PosX">検索を行うX座標</param>
    /// <param name="PosZ">検索を行うZ座標</param>
    /// <returns></returns>
    public bool CheckBelow(int PosX, int PosZ)
    {
        if (map[PosZ+1, PosX] == empty)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// マップ上に隕石が存在するかどうか
    /// </summary>
    /// <returns></returns>
    public bool CheckMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                if (map[z, x] == meteor)
                {
                    return false;
                }
            }
        }
        return true;
    }
}