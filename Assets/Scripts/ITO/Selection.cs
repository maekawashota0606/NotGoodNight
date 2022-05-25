using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    //[SerializeField] Image gameObject;
    public bool i = false;

    GameObject image_object = null;

    Image image_component = null;

    void OnMouseOver()
    {
        i = true;
        Debug.Log("OnMouseOver");
    }
    void OnMouseExit()
    {
        i = false;
        Debug.Log("OnMouseExit");
    }

    
    void Start()
    {
        // �I�u�W�F�N�g�̎擾
        image_object = GameObject.Find("Image");
        // �R���|�[�l���g�̎擾
        image_component = image_object.GetComponent<Image>();
        
    }
    void Update()
    {
        if (i == true)
        {
            image_component.color = Color.red;
            
        }
        else
        {
            image_component.color = Color.white;
        }
    }
    
}
    