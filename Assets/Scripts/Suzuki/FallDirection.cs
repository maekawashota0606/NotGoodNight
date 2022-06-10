using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDirection : MonoBehaviour
{
    public List<GameObject> meteors = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            for(int a = 0; a <= 10; a++)
            {
                this.transform.Translate(Vector2.left * -1);
            }
        }
    }
}
