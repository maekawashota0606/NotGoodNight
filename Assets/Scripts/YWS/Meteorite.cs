using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    /// <summary>
    /// 隕石の移動
    /// </summary>
    public void Move()
    {
        this.transform.position += new Vector3(0,0,-1);
    }

    /// <summary>
    /// 隕石の破壊
    /// </summary>
    public void Destroy()
    {
        
    }
}