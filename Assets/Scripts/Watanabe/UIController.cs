using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    void OnMouseDown()
    {
        Camera.main.gameObject.GetComponent<ShakeCamera>().Shake();
    }

}