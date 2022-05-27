using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] GameObject _particalObj = default(GameObject);

    public MyVector3 _myVector3;

    private void Start()
    {

    }
    
    /// <summary>
    /// 隕石が移動を行った時
    /// </summary>
    public void onMoved(Vector3 movedPos)
    {
        _myVector3.x = movedPos.x;
        _myVector3.y = movedPos.y;
        _myVector3.z = movedPos.z;
    }


    /// <summary>
    /// 隕石の破壊
    /// </summary>
    public void OnDestroy()
    {
        
    }
}