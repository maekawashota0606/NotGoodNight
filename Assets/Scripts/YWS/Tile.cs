using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField, Header("点滅させるオブジェクト")] private SpriteRenderer tile = null;
    private float alpha_Sin = 0.0f;
    private bool ShouldBlink = false;
    
    private void Start()
    {
        // 値を初期化
        tile.color = new Color(255,255,255,0);
    }

    void Update()
    {
        alpha_Sin = Mathf.Sin(Time.time) / 2 + 0.5f;
    }

    private IEnumerator ColorCoroutine()
    {
        yield return new WaitUntil(() => ShouldBlink == true);

        Color _color = tile.material.color;
        _color.a = alpha_Sin;
        tile.material.color = _color;
    }

    private void OnMouseOver()
    {
        ShouldBlink = true;
        StartCoroutine(ColorCoroutine());
    }

    private void OnMouseExit()
    {
        ShouldBlink = false;
    }
}