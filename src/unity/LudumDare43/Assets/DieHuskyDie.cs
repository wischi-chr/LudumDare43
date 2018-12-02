using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHuskyDie : MonoBehaviour {
    public bool Killable = false;
    public ParticleSystem ParticleSys;
    public GameObject GameWorld;

    private Transform gameTransform;
    public bool Dead = false;
    private SpriteRenderer info;

    // Use this for initialization
    void Start () {
        gameTransform = GameObject.Find("Game").GetComponent<Transform>();
        info = this.transform.Find("info").gameObject.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Guy")
        {
            Killable = true;
            Debug.Log(this.name + ": " + Killable);
            info.enabled = true && !Dead;
        }
        
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Guy")
        {
            Killable = false;
            Debug.Log(this.name + ": " + Killable);
            info.enabled = false;
        }
    }

    public void Kill()
    {
        ParticleSys.Play();
        var animator = GetComponent<Animator>();
        animator.SetBool("Dead", true);

        GlobalGameState.Food = 1f;

        this.transform.parent = gameTransform;

        Dead = true;
        GlobalGameState.DogsAlive--;
    }


}
