using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField, Header("カーソルオブジェクト")] private GameObject cursorObj = null;
    private SpriteRenderer cursor = null;
    [SerializeField, Header("カーソル画像")] private Sprite[] CursorImage = new Sprite[2];
    private Vector3 mouse = Vector3.zero;
    private Vector3 target = Vector3.zero;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBoot()
    {
        Debug.Log("<color=red>カーソルマネージャー運転中</color>");
    }

    private void Awake()
    {
        //シーン遷移では破棄させない
        DontDestroyOnLoad(gameObject);
        cursor = cursorObj.GetComponent<SpriteRenderer>();
        //マウスカーソルの表示をなくす
        Cursor.visible = false;
        //カーソルとして使うオブジェクトを起動する
        cursorObj.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //マウスカーソルの位置を随時更新
        mouse = Input.mousePosition;
        target = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10));
        //取得したマウスカーソルの位置にカーソルオブジェクトを移動させる
        cursorObj.transform.position = target;

        //クリックを行った場合、カーソルオブジェクトの画像を切り替える
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            StartCoroutine(ChangeCursorImage());
        }   
    }

    /// <summary>
    /// カーソルオブジェクトの画像を切り替える処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeCursorImage()
    {
        cursor.sprite = CursorImage[1];
        yield return new WaitForSeconds(0.25f);
        cursor.sprite = CursorImage[0];
    }
}