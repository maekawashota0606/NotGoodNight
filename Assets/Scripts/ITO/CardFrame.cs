using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardFrame : MonoBehaviour
{
    //[SerializeField] Image gameObject;
    public bool i = false;
    public bool l = false;
    public bool r = false;
    bool isCalledOnce = false;
    bool isCalledOnce2 = false;

    [SerializeField]
    Image image_component = null;

    void OnMouseOver()
    {
        i = true;
        //Debug.Log("OnMouseOver");
    }
    void OnMouseExit()
    {
        i = false;
        //Debug.Log("OnMouseExit");
    }
    void OnMouseDown()
    {
        l = true;
        isCalledOnce2 = false;
    }
    
    


    void Start()
    {
        
    }
    void Update()
    {
        if (!isCalledOnce2)
        {
            if (l == true)
            {
                image_component.color = Color.red;
                isCalledOnce = true;
            }
        }
        if (Input.GetMouseButtonDown(1)&& i == true)
        {
            image_component.color = Color.white;
            isCalledOnce2 = true;
            isCalledOnce = false;
        }
        if (!isCalledOnce)
        {
            

            if (i == true)
            {
                image_component.color = Color.yellow;
                
            }
            else
            {
                image_component.color = Color.white;
                
            }
        }
            
    }
    
}
    