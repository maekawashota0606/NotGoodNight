using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Meteor = null;

    // Start is called before the first frame update
    void Start()
    {
        NewMeteor();
    }

    void NewMeteor()
    {
        Instantiate(Meteor, transform.position, Quaternion.identity);
    }
}
