using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHuskyDie : MonoBehaviour {
    public bool Killable = false;
    public ParticleSystem ParticleSys;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.name + ": " + Killable);
		if(Killable && Input.GetKeyDown(KeyCode.F))
        {
            Kill();
        }
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Guy")
        {
            Killable = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Guy")
        {
            Killable = false;
        }
    }

    private void Kill()
    {
        ParticleSys.Play();
        var animator = GetComponent<Animator>();
        animator.SetBool("Dead", true);

        GlobalGameState.Food = 1f;
    }
}
