using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDirection : MonoBehaviour
{
    public List<GameObject> meteors = new List<GameObject>();

    private float Times;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Transform Trans = this.transform;

            Times = Time.deltaTime;


            Vector3 pos = Trans.position;
            pos.z -= 0.1f;
            Trans.position = pos;

            /*for(int a = 0; a <= 10; a++)
            {
                this.transform.position();
            }*/
        }
    }
}
