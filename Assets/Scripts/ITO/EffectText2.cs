using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectText2 : MonoBehaviour
{
    public Text TextEffect;
    public Text TextName;
    public Text TextCost;
    public Image Illustration;
    [SerializeField] private Sprite[] IllustrationCards = new Sprite[35];

void Start()
    {
        
    }

    void Update()
    {
        if (GameDirector.Instance.SelectedCardObject != null)
        {
            TextEffect.text = GameDirector.Instance.SelectedCardObject.EffectText;
            TextName.text = GameDirector.Instance.SelectedCardObject.Name;
            TextCost.text = GameDirector.Instance.SelectedCardObject.Cost.ToString();
            if(IllustrationCards[GameDirector.Instance.SelectedCardObject.ID - 1] != null)
            {
                Illustration.sprite = IllustrationCards[GameDirector.Instance.SelectedCardObject.ID - 1];
            }
            
        }
        //Debug.Log(TextFrame.text); 
    }

}
