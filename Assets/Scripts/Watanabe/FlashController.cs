using UnityEngine;
using UnityEngine.UI;

public class FlashController : MonoBehaviour
{
    private Image img;

    void Start()
    {
        img = GetComponent<Image>();
        img.color = Color.clear;
    }
    [SerializeField, Range(0f, 1f)]
    public float FlashTime = 1f;
    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            img.color = new Color(1,1,1,1);
        }
        else
        {
            img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * FlashTime);
        }
    }
}