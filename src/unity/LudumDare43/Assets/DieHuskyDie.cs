using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieHuskyDie : MonoBehaviour {
    public bool Killable = false;
    public ParticleSystem ParticleSys;
    public GameObject GameWorld;
    public AudioClip whine;

    private Transform gameTransform;
    public bool Dead = false;
    private SpriteRenderer info;
    private AudioSource audioSource;
    
    // Use this for initialization
    void Start () {
        gameTransform = GameObject.Find("Game").GetComponent<Transform>();
        info = this.transform.Find("info").gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Guy" && this.name == GlobalGameState.NextKillName)
        {
            Killable = true;
            Debug.Log(this.name + ": " + Killable);
            info.enabled = true && !Dead;
            audioSource.Play();
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
        audioSource.clip = whine;
        audioSource.Play();

        ParticleSys.Play();
        var animator = GetComponent<Animator>();
        animator.SetBool("Dead", true);

        GlobalGameState.Food = 1f;

        this.transform.parent = gameTransform;

        Dead = true;
        GlobalGameState.DogsAlive--;
        GlobalGameState.KillIndex++;
    }


}
