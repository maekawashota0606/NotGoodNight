using UnityEngine;
using System.Collections;

public class AnimationSingleTest : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		Animation anim = gameObject.GetComponent<Animation>();
		PlayAnimation(anim);
	}

	void PlayAnimation(Animation anim)
	{
		anim.Play();
	}

}
