using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D compRigidBody;
    private float distToGround;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public float movementSpeed = 100f;
    private float sprintMultiplyer = 2;
    public float JumpHeight = 250;
    public bool DebugInformation = true;
    public int PossibleJumps = 1;
    public int ExecutedJumps = 0;
    public float JumpRayCast = 0.75f;

    public LayerMask GroundLayer;


    private bool jumping = false;
    private Vector2 lookDirection = Vector2.right;

    // Use this for initialization
    void Start()
    {
        compRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        float sprint = 1;

        if (Input.GetKey(sprintKey))
            sprint = sprintMultiplyer;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f * sprint;

        lookDirection = x > 0 ? Vector2.right : Vector2.left;


        var grounded = IsGrounded();

        if (Input.GetKeyDown(jumpKey) && grounded && !jumping)
        {
            Jump();
            jumping = true;
            ExecutedJumps = 1;
        }
        else if (!jumping && Input.GetKeyDown(jumpKey) && ExecutedJumps < PossibleJumps)
        {
            Jump();
            ExecutedJumps++;
        }

        if (!Input.GetKey(jumpKey))
        {
            jumping = false;
        }

        //velocity method
        var vel = (transform.right * x) * movementSpeed;
        vel.y = compRigidBody.velocity.y;
        compRigidBody.velocity = vel;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        if (DebugInformation)
        {
            DrawDebugInformation();
        }
    }

    private void Jump()
    {
        compRigidBody.AddForce(Vector2.up * JumpHeight);
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, JumpRayCast, GroundLayer).collider != null
            || Physics2D.Raycast(transform.position - new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, GroundLayer).collider != null
            || Physics2D.Raycast(transform.position + new Vector3(0.5f, 0, 0), Vector2.down, JumpRayCast, GroundLayer).collider != null;
    }

    private void DrawDebugInformation()
    {
        Debug.DrawRay(transform.position, Vector2.down * JumpRayCast, Color.green);
        Debug.DrawRay(transform.position - new Vector3(0.5f, 0, 0), Vector2.down * JumpRayCast, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0, 0), Vector2.down * JumpRayCast, Color.green);

        Debug.DrawRay(transform.position, lookDirection * 2, Color.blue);
    }
}