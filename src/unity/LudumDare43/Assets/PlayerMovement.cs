using Assets;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D compRigidBody;
    private Transform playerTransform;
    private Transform sleighTransform;
    private Transform gameTransform;

    private float distToGround;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode interactionKey = KeyCode.F;

    public float movementSpeed = 100f;
    private float sprintMultiplyer = 2;
    public float JumpHeight = 1f;
    public bool DebugInformation = true;
    public int PossibleJumps = 1;
    public int ExecutedJumps = 0;
    public float JumpRayCast = 0.75f;

    public LayerMask GroundLayer;
    public SleighMovementController sleighController;

    public float sleighTargetVelocity = 0f;
    private bool jumping = false;
    private Vector2 lookDirection = Vector2.right;
    private bool sleighIsParent = false;

    // Use this for initialization
    void Start()
    {
        compRigidBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        sleighTransform = sleighController.GetComponent<Transform>();
        gameTransform = GameObject.Find("Game").GetComponent<Transform>();

        sleighController.IsEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        GlobalGameState.EndXPosition = transform.position.x;

        float sprint = 1;

        if (Input.GetKey(sprintKey))
            sprint = sprintMultiplyer;

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f * sprint;

        lookDirection = x > 0 ? Vector2.right : Vector2.left;

        var isJumpKeyDown = Input.GetKeyDown(jumpKey) || Input.GetAxis("Vertical") > float.Epsilon;

        var isOnSleight = IsOnElement("sleigh");
        var isOnFloor = IsOnElement("floor");
        var grounded = isOnSleight || isOnFloor;

        if (isOnSleight && !isOnFloor)
        {
            playerTransform.parent = sleighTransform;
            sleighIsParent = true;
        }
        else if (isOnFloor && !isOnSleight)
        {
            playerTransform.parent = gameTransform;
            sleighIsParent = false;
            sleighTargetVelocity = 0;
        }

        if (sleighIsParent && Input.GetKeyDown(interactionKey))
        {
            sleighTargetVelocity += 0.3f;
        }

        sleighTargetVelocity -= sleighTargetVelocity * 0.1f * Time.deltaTime;

        if (sleighTargetVelocity < 0)
            sleighTargetVelocity = 0;

        sleighController.acceleration = GlobalGameState.DogsAlive / 3.0f;
        sleighController.targetVelocity = sleighTargetVelocity;

        if (compRigidBody.velocity.y > 0.01f)
            isJumpKeyDown = false;

        var targetYVel = compRigidBody.velocity.y;

        if (isJumpKeyDown && grounded)
            targetYVel = JumpHeight;

        targetYVel = Mathf.Min(targetYVel, JumpHeight);

        compRigidBody.velocity = new Vector2(x * movementSpeed, targetYVel);

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        if (DebugInformation)
        {
            DrawDebugInformation();
        }
    }

    private bool IsOnElement(string name)
    {
        var colliders = new Collider2D[5];
        var len = compRigidBody.GetContacts(colliders);

        for (int i = 0; i < len; i++)
        {
            if (colliders[i].GetComponent<BoxCollider2D>().name == name)
                return true;
        }

        return false;
    }

    private void DrawDebugInformation()
    {
        Debug.DrawRay(transform.position, Vector2.down * JumpRayCast, Color.green);
        Debug.DrawRay(transform.position - new Vector3(0.5f, 0, 0), Vector2.down * JumpRayCast, Color.green);
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0, 0), Vector2.down * JumpRayCast, Color.green);

        Debug.DrawRay(transform.position, lookDirection * 2, Color.blue);
    }
}