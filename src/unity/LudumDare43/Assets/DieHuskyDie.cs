using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHuskyDie : MonoBehaviour {

    public ParticleSystem ParticleSys;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Guy")
        {
            //Destroy(gameObject);
            ParticleSys.Play();
            var animator = GetComponent<Animator>();
            animator.SetBool("Dead", true);
        }
    }
}
