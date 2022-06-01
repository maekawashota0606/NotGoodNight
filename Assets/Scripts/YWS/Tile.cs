using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Header("点滅させるオブジェクト")] private SpriteRenderer tile = null;
    [SerializeField, Header("点滅させるスピード")] private float _blinkSpeed = 0.0f;

    // 時間計測変数
    private float _sceneTime = 0.0f;

    private bool ShouldBlink = false;
    
    private void Start()
    {
        // 値を初期化
        tile.color = GetAlphaColor(tile.color);
    }

    // Update is called once per frame
    void Update()
    {
        tile.color = GetAlphaColor(tile.color);

        //マウスが離れたら透明に戻る
        if (ShouldBlink == false)
        {
            tile.color = new Color(255, 255, 255, 0);
        }
    }

    //Alpha値を更新してColorを返す
    public Color GetAlphaColor(Color color)
    {
        _sceneTime += Time.deltaTime * 5.0f * _blinkSpeed;
        color.a = Mathf.Sin(_sceneTime) * 0.5f + 0.5f;

        return color;
    }

    private void OnMouseOver()
    {
        ShouldBlink = true;
    }

    private void OnMouseExit()
    {
        ShouldBlink = false;
    }
}
