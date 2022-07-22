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

    public void CheckUp(int x, int z)
    {
        if (z == 0)
        {
            return;
        }

        if (map[z-1, x] == meteor)
        {
            TileMap.Instance.tileMap[x, z-1].tag = "Area";
            CheckUp(x, z-1);
            CheckDown(x, z-1);
            CheckLeft(x, z-1);
            CheckRight(x, z-1);
        }
    }

    public void CheckDown(int x, int z)
    {
        if (z == 9)
        {
            return;
        }

        if (map[z+1, x] == meteor)
        {
            TileMap.Instance.tileMap[x, z+1].tag = "Area";
            CheckUp(x, z+1);
            CheckDown(x, z+1);
            CheckLeft(x, z+1);
            CheckRight(x, z+1);
        }
    }

    public void CheckLeft(int x, int z)
    {
        if (x == 0)
        {
            return;
        }

        if (map[z, x-1] == meteor)
        {
            TileMap.Instance.tileMap[x-1, z].tag = "Area";
            CheckUp(x-1, z);
            CheckDown(x-1, z);
            CheckLeft(x-1, z);
            CheckRight(x-1, z);
        }
    }

    public void CheckRight(int x, int z)
    {
        if (x == 9)
        {
            return;
        }

        if (map[z, x+1] == meteor)
        {
            TileMap.Instance.tileMap[x+1, z].tag = "Area";
            CheckUp(x+1, z);
            CheckDown(x+1, z);
            CheckLeft(x+1, z);
            CheckRight(x+1, z);
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

    /// <summary>
    /// マップデータ確認
    /// </summary>
    public void CheckMapData()
    {
        string printMapData = "";
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				printMapData += map[i, j].ToString() + ",";
			}
			printMapData += "\n";
		}
		Debug.Log("マップ\n" + printMapData);
    }
}