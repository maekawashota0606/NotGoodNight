using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    //経過時間
    private float elapsedTime = 0f;
    [SerializeField, Header("目標時間")] private float TargetTime = 0.5f;
    [SerializeField] private float Duration = 2f;
    public bool DoNextTurn = false;
    //移動が開始する前の座標を保存しておく変数
    private int StartXPosition = 0;
    private int StartZPosition = 0;
    private float TargetXPosition = 0;
    private float TargetZPosition = 0;
    //このターン落下を行っているかどうか
    public bool FallFinished = false;
    public bool MoveFinished = false;
    [SerializeField] private Animator meteorAnimator = null;
    private bool DoMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Search" || TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Area")
        {
            meteorAnimator.SetBool("IsArea", true);
        }
        else
        {
            meteorAnimator.SetBool("IsArea",false);
        }

        if (GameDirector.Instance.gameState == GameDirector.GameState.active)
        {
            FallFinished = false;
            MoveFinished = false;
        }

        if (DoMoving)
        {
            Vector3 targetPos = new Vector3(TargetXPosition, 0, TargetZPosition);
            elapsedTime += Time.deltaTime;
            this.transform.position = Vector3.Lerp(this.transform.position, targetPos, elapsedTime / Duration);
            if (elapsedTime >= Duration)
            {
                elapsedTime = 0;
                this.transform.position = new Vector3(TargetXPosition, 0, TargetZPosition);
                MoveFinished = true;
                DoMoving = false;
            }
        }

        //落下の演出
        if (DoNextTurn)
        {
            elapsedTime += Time.deltaTime;
            this.transform.position += new Vector3(0, 0, -1.0f * Time.deltaTime * 2);
            if (elapsedTime >= TargetTime)
            {
                elapsedTime = 0;
                this.transform.position = new Vector3(StartXPosition, 0, StartZPosition - 1);
                FallFinished = true;
                DoNextTurn = false;
            }
        }
    }

    /// <summary>
    /// 落下を開始する
    /// </summary>
    public void StartFall()
    {
        if (FallFinished == false)
        {
            StartXPosition = (int)this.transform.position.x;
            StartZPosition = (int)this.transform.position.z;
            DoNextTurn = true;
            //マップの元居た場所の記録を削除し
            Map.Instance.map[StartZPosition*-1, StartXPosition] = Map.Instance.empty;
            //マップの移動先に新たに記録を書き込む
            Map.Instance.map[StartZPosition*-1+1, StartXPosition] = Map.Instance.meteor;
        }
    }

    public void MoveToTargetPoint(int PosX, int PosZ)
    {
        if (MoveFinished == false)
        {
            StartXPosition = (int)this.transform.position.x;
            StartZPosition = (int)this.transform.position.z;
            TargetXPosition = PosX;
            TargetZPosition = PosZ;
            DoMoving = true;
        }
    }
}