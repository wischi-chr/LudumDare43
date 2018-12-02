using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOffset : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var anim = GetComponent<Animator>();

        GetComponent<Animator>().SetFloat("Offset", Random.Range(0.0f, 1.0f));
    }

}
