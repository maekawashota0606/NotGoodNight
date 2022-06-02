using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject root = null;

    private void Start()
    {
        GenreateGrid();
    }

    void GenreateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                GameObject spawnedTile = Instantiate(_tilePrefab);
                spawnedTile.transform.parent = root.transform;
                spawnedTile.transform.position = new Vector3(x,y);
                spawnedTile.name = $"tile {x} {y}";
                TileMap.Instance.map[y,x] = spawnedTile;
            }
        }
    }
}
