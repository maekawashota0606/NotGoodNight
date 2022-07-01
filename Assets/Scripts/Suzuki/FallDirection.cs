using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDirection : MonoBehaviour
{
    public List<GameObject> meteors = new List<GameObject>();

    private float Times = 0f;
    public float Timev = 1f;

    public bool DoNextTurn = false;

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
            Vector3 pos = Trans.position;
            pos.z -= 0.1f;
            Trans.position = pos;

            /*for(int a = 0; a <= 10; a++)
            {
                this.transform.position();
            }*/

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            DoNextTurn = true;
        }


        if (DoNextTurn)
        {
            Times += Time.deltaTime;

            this.transform.position += new Vector3(0, 0, -1.0f * Time.deltaTime);

            if (Times >= Timev)
            {
                Times = 0;
                DoNextTurn = false;
            }
        }


    }
}
