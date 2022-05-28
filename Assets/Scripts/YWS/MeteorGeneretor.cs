using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeteorGeneretor : MonoBehaviour
{
    [SerializeField] private GameObject meteorPrefab = null;

    public void Generate(Vector3 GenPos)
    {
        Vector3 Pos = transform.position + GenPos;

        Instantiate(meteorPrefab, Pos, Quaternion.identity);
    }
}