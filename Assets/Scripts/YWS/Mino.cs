using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;
    //落ちる時間
    public float fallTime = 1f;

    //ステージの大きさ
    private static int width = 10;
    private static int height = 20;

    //回転
    public Vector3 rotationPoint;

    //grid
    private static Transform[,] grid = new Transform[width,height];

    void Update()
    {
        MinoMovement();        
    }

    private void MinoMovement()
    {
        //左
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1,0,0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1,0,0);
            }
        }
        //右
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1,0,0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(1,0,0);
            }
        }
        //下
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0,-1,0);

            if (!ValidMovement())
            {
                transform.position -= new Vector3(0,-1,0);
                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0,0,1), 90);
        }
    }

    //ラインがあるか？確認
    public void CheckLines()
    {
        for (int i = height - 1; i > 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    //列がそろっているか確認
    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j,i] == null)
            {
                return false;
            }
        }
        return true;
    }

    //ラインを消す
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j,i].gameObject);
            grid[j,i] = null;
        }
    }

    //列を下げる
    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j,y] != null)
                {
                    grid[j, y - 1] = grid[j,y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0,1,0);
                }
            }
        }
    }

    void AddToGrid()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
        }   
    }

    //移動範囲の制御
    bool ValidMovement()
    {
        foreach(Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //minoがステージよりはみ出さないように制御
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }
            if (grid[roundX,roundY] != null)
            {
                return false;
            }
        }
        return true;
    }
}