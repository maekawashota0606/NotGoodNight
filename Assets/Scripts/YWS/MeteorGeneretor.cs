using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorGeneretor : MonoBehaviour
{
    [SerializeField]
    private GameObject root = null;
    [SerializeField]
    private GameObject meteorPrefab = null;

    public void Generate(Vector3 GeneratePos, out GameObject meteorObj)
    {
        // コマ1つ目処理
        GameObject meteor = Instantiate(meteorPrefab);
        //meteor.transform.parent = root.transform;
        meteor.transform.position = GeneratePos;
        Meteorite p1 = meteor.GetComponent<Meteorite>();

        // out引数のため代入
        meteorObj = meteor;

        // 代入
        GameDirector.Instance._activePieces[0] = meteor;
    }
}