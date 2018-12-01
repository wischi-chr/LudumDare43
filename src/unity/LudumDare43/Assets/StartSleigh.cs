using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSleigh : MonoBehaviour {
    public LayerMask SleighLayer;
    public float JumpRayCast = 0.75f;
    public GameObject Sleigh;
    private Rigidbody2D sleighRigidBody;
    public int Speed = 200;

    // Use this for initialization
    void Start () {
        sleighRigidBody = Sleigh.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        var onSleigh = IsOnSleigh();

        if(onSleigh)
        {
            // TODO: Move Sleigh
        }
    }


    private bool IsOnSleigh()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, JumpRayCast, SleighLayer).collider != null
            || Physics2D.Raycast(transform.position - new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, SleighLayer).collider != null
            || Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, SleighLayer).collider != null;
    }
}
