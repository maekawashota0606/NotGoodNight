using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    [SerializeField] private GameObject root = null;
    [SerializeField] private GameObject meteorPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        Map.Instance.CheckMap();
        //Generate();
        Map.Instance.CheckMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            MeteorMove();
        }
    }

    private void Generate(Vector3 GeneratePos)
    {
        GameObject meteor = Instantiate(meteorPrefab);
        meteor.transform.parent = root.transform;
        meteor.transform.position = GeneratePos;
    }

    public void MeteorMove()
    {

    }
}
