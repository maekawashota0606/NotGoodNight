using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeteoAnimasion : MonoBehaviour
{
    [SerializeField] private GameObject Meteo = null;
    public Sprite Meteos;
    /*public Sprite FirstImage;
    public Sprite SecondImage;
    public Sprite ThirdImage;
    private float fps = 0.1f;
    public GameObject MuneImage;*/
    [SerializeField] Animator animator;

    static readonly int hashStatePositive = Animator.StringToHash("Positive");
    static readonly int hashStateNegative = Animator.StringToHash("Negative");


    /*void Start()
    {
        MuneImage = GameObject.Find("Image").GetComponent<Image>();
    }*/

    // Update is called once per frame
    void Update()
    {
        if (TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Search" ||
                        TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Area")
        {
            //StartCoroutine("Sample");
            animator.Play(hashStatePositive);
        }
        else if (TileMap.Instance.tileMap[(int)this.transform.position.x, (int)this.transform.position.z * -1].tag == "Untagged")
        {
            //StopCoroutine(Sample());
            animator.Play(hashStateNegative);
        }
    }
    /*public void MeteoAnimaterRiset()
    {
        var spriteRenderer = Meteo.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = Meteos;
    }*/
    /*IEnumerator Sample()
    {
        var spriteRenderer = Meteo.GetComponent<SpriteRenderer>();
        while (true)
        {
            yield return new WaitForSeconds(fps);
            spriteRenderer.sprite = FirstImage;

            yield return new WaitForSeconds(fps);
            spriteRenderer.sprite = SecondImage;

            yield return new WaitForSeconds(fps);
            spriteRenderer.sprite = ThirdImage;
        }
    }*/
}
