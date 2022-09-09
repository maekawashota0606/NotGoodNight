using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField, Header("カーソル画像")] private Texture2D[] CursorImage = new Texture2D[2];

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBoot()
    {
        Debug.Log("<color=red>カーソルマネージャー運転中</color>");
    }

    private void Awake()
    {
        // 音管理はシーン遷移では破棄させない
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ChangeCursorImage());
        }   
    }

    private IEnumerator ChangeCursorImage()
    {
        Cursor.SetCursor(CursorImage[1], Vector2.zero, CursorMode.Auto);
        yield return new WaitForSeconds(0.25f);
        Cursor.SetCursor(CursorImage[0], Vector2.zero, CursorMode.Auto);
    }
}
