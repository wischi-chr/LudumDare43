using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSleigh : MonoBehaviour {
    public LayerMask SleighLayer;
    public float JumpRayCast = 0.75f;
    public GameObject Sleigh;
    private Rigidbody2D sleighRigidBody, playerRigidBody;
    public int Speed = 200;
    public List<GameObject> Huskies;
    public List<Animator> HuskyAnimators;

    // Use this for initialization
    void Start () {
        sleighRigidBody = Sleigh.GetComponent<Rigidbody2D>();

        foreach(var husky in Huskies)
        {
            HuskyAnimators.Add(husky.GetComponent<Animator>());
        }
    }
	
	// Update is called once per frame
	void Update () {
        var onSleigh = IsOnSleigh();

        if(onSleigh)
        {
            foreach(var animator in HuskyAnimators)
            {
                animator.SetFloat("Speed", Mathf.Abs(10), Random.Range(0.0f, 0.2f), Time.deltaTime);
            }

            var vel = (Sleigh.transform.right * 1) * Speed;
            vel.y = sleighRigidBody.velocity.y;
            //sleighRigidBody.velocity = vel;
            //var x = Sleigh.transform.TransformDirection(Vector3.forward) * 1;
        } else
        {
            foreach (var animator in HuskyAnimators)
            {
                animator.SetFloat("Speed", Mathf.Abs(0), Random.Range(0.0f, 0.2f), Time.deltaTime);
            }

            sleighRigidBody.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
    }

    //void OnTriggerEnter2D(Collider2D col)
    //{
    //    Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    //    Debug.Log("Trigger");
    //    var animator = col.GetComponent<Animator>();
    //    animator.SetBool("Dead", true);
    //}

    private bool IsOnSleigh()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, JumpRayCast, SleighLayer).collider != null
            || Physics2D.Raycast(transform.position - new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, SleighLayer).collider != null
            || Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, SleighLayer).collider != null;
    }
}
