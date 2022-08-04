using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class MeteorGenerator : MonoBehaviour
{
    //親オブジェクト
    [SerializeField] private GameObject root = null;
    //生成するプレハブ
    [SerializeField] private GameObject meteorPrefab = null;

    public void Generate(Vector3 GenPos)
    {
        //プレハブを生成
        GameObject genMeteor = Instantiate(meteorPrefab);
        //生成したオブジェクトを親オブジェクトの下に置く
        genMeteor.transform.parent = root.transform;
        //生成したオブジェクトを所定位置に付かせる
        genMeteor.transform.position = GenPos;
        Meteorite newMeteor = genMeteor.GetComponent<Meteorite>();
        //ディレクター側にある隕石のリストに入れる
        GameDirector.Instance.meteors.Add(newMeteor);
    }
}