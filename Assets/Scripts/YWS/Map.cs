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
        int meteorNum = 0;
        string printMapData = "";
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 10; j++)
			{
				printMapData += map[i, j].ToString() + ",";
                //マップデータ上で隕石を確認した場合、カウンターを増やす
                if (map[i, j] == meteor)
                {
                    meteorNum++;
                }
			}
			printMapData += "\n";
		}
		Debug.Log("マップ\n" + printMapData);
        //マップデータで確認できた隕石の数と実際の隕石の数が異なっている場合
        if (meteorNum != GameDirector.Instance.meteors.Count)
        {
            Debug.LogError("The number of meteor on map data is not match with the real number of meteor");
            //マップデータを再登録する
            UpdateMapData();
        }
        else
        {
            Debug.Log("Meteors number match");
        }
    }

    /// <summary>
    /// マップデータを再登録する関数
    /// </summary>
    public void UpdateMapData()
    {
        //マップデータを初期化
        map = new string[height,width]
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

        //実際に存在している隕石をマップデータの該当座標に登録する
        for (int num = 0; num < GameDirector.Instance.meteors.Count; num++)
        {
            map[(int)GameDirector.Instance.meteors[num].transform.position.z * -1, (int)GameDirector.Instance.meteors[num].transform.position.x] = meteor;
        }
        //ダブルチェック
        CheckMapData();
    }
}