using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 自作のVector3構造体
[System.Serializable]
public struct MyVector3
{
    // コンストラクタ
    public MyVector3(Vector3 vector3)
    {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }
    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    public float x;
    public float y;
    public float z;

    /// <summary>
    /// 自作Vector3 -> UnityVector3 に変換する関数
    /// </summary>
    /// <returns></returns>
    public Vector3 ToVector3()
    {
        return new Vector3(x,y,z);
    }
}

public static class Vector3ext
{
    /// <summary>
    /// UnityVector3 -> 自作Vector3 に変換する変数
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public static MyVector3 ToMyVector3(this Vector3 vector3)
    {
        return new MyVector3(vector3);
    }
}