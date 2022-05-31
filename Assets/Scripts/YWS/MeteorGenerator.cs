using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorGenerator : MonoBehaviour
{
    //親オブジェクト
    [SerializeField] private GameObject root = null;
    //生成するプレハブ
    [SerializeField] private GameObject meteorPrefab = null;

    public void Generate(Vector3 GenPos)
    {
        //プレハブを生成
        GameObject meteor = Instantiate(meteorPrefab);
        //生成したオブジェクトを親オブジェクトの下に置く
        meteor.transform.parent = root.transform;
        //生成したオブジェクトを所定位置に付かせる
        meteor.transform.position = GenPos;
        //ディレクター側にある隕石のリストに入れる
        GameDirector.Instance.meteors.Add(meteor);
    }
}