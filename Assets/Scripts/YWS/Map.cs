using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : SingletonMonoBehaviour<Map>
{
    public string[,] map = new string[11, 10] // z, x座標で指定
    {
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"□", "□", "□", "□", "□", "□", "□", "□", "□", "□"},
        {"x", "x", "x", "x", "x", "x", "x", "x", "x", "x"},
    };

    public readonly string empty = "□";
    public readonly string deleteZone = "x";
    public readonly string meteor = "o";
    
    /// <summary>
    /// 移動後のコマが障害物に当たるかを調べる
    /// </summary>
    /// <param name="movedPos">コマの移動後座標</param>
    /// <returns></returns>
    public bool CheckEmpty(Vector3 movedPos)
    {
        bool isBlank = false;
        // ミノの移動後x, y座標
        int z = (int) movedPos.z * -1; // zは基本0以下になるので符号を反転させ配列を指定する
        int x = (int) movedPos.x;

        // 2つの駒の移動後座標に何もなければ移動を通す
        if (map[z, x] == empty)
            isBlank = true;

        return isBlank;
    }
    
    /// <summary>
    /// コマの１つ下のマスが空いていないかを調べる
    /// </summary>
    /// <param name="piecePos"></param>
    /// <returns></returns>
    public bool CheckLanding(Vector3 piecePos)
    {
        bool isGrounded = false;

        // ミノの移動後座標
        int z = (int) piecePos.z * -1;
        int x = (int) piecePos.x;

        if (map[z + 1, x] != empty)
            isGrounded = true;

        return isGrounded;
    }

    /// <summary>
    /// 着地後判定処理関数
    /// </summary>
    /*public void FallPiece(GameObject piece)
    {
        // 配列指定子用のコマの座標         
        int x = (int) piece.transform.position.x;
        int z = (int) piece.transform.position.z * -1; // zはマイナス方向に進むので符号を反転させる

        int i = 0;
        int dz = 0;

        while (true)
        {
            i++;
            dz = z + i; // iの分だけ下の座標を調べる

            // 設置したマスからi個下のマスが空白なら下に落とす
            if (map[dz, x] == empty)
                piece.transform.position = new Vector3(x, 0, dz * -1); // 反転させたyをマイナスに戻す
            else
            {
                dz--;
                break;
            }
            // これを空白以外に当たるまで繰り返す
        }

        Piece p = piece.GetComponent<Piece>();

        // mapに記録
        switch (p.pieceType)
        {
            case Piece.PieceType.black:
                map[dz, x] = black;
                break;
            case Piece.PieceType.white:
                map[dz, x] = white;
                break;
            case Piece.PieceType.fixityBlack:
                map[dz, x] = fixityBlack;
                break;
            case Piece.PieceType.fixityWhite:
                map[dz, x] = fixityWhite;
                break;
            default:
                break;
        }

        pieceMap[dz, x] = piece;
        
    }*/
}

// map確認用
//for (int a = _EMPTY_AREAS_HEIGHT; a < _HEIGHT; a++)
//{
//    string s = "";
//    for (int b = 0; b<_WIDTH; b++)
//    {
//        s += _map[a, b];
//    }
//    Debug.Log(s);
//