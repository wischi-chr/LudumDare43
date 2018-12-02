using Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CameraFollowObject cameraFollow;
    private Rigidbody2D compRigidBody;
    private Transform playerTransform;
    private Transform sleighTransform;
    private Transform gameTransform;
    private HuskyTestScript playerWalkingAnimations;

    private float distToGround;

    public GameObject RestartButton;
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
    private bool playerDead = false;
    private Animator playerAnimator;
    private SpriteRenderer info;
    private List<DieHuskyDie> dieList = new List<DieHuskyDie>();

    // Use this for initialization
    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollowObject>();
        compRigidBody = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        sleighTransform = sleighController.GetComponent<Transform>();
        gameTransform = GameObject.Find("Game").GetComponent<Transform>();
        playerWalkingAnimations = GetComponent<HuskyTestScript>();
        playerAnimator = GetComponent<Animator>();
        info = sleighTransform.Find("info").gameObject.GetComponent<SpriteRenderer>();

        sleighController.IsEnabled = true;

        FindHuskies();
    }

    void FindHuskies()
    {
        var cnt = 0;

        foreach (Transform child in sleighTransform)
        {
            if (Array.IndexOf(GlobalGameState.DogNames, child.gameObject.name) > -1)
            {
                dieList.Add(child.gameObject.GetComponent<DieHuskyDie>());
                cnt++;
            }
        }

        Debug.Log("Huskies found: " + cnt);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalGameState.Food < 3 * float.Epsilon && !playerDead)
        {
            //STARVED TO DEATH
            playerDead = true;
            sleighTargetVelocity = 0f;
            playerAnimator.SetBool("Dead", true);
            RestartButton.SetActive(true);
        }

        if (!playerDead)
            GlobalGameState.EndXPosition = transform.position.x;

        float sprint = 1;

        if (GetKey(sprintKey))
            sprint = sprintMultiplyer;

        var x = GetAxisInput("Horizontal") * Time.deltaTime * 3.0f * sprint;

        lookDirection = x > 0 ? Vector2.right : Vector2.left;

        var isJumpKeyDown = GetKeyDown(jumpKey) || GetAxisInput("Vertical") > float.Epsilon;

        var isOnSleight = IsOnElement("sleigh");
        var isOnFloor = IsOnElement("floor");
        var grounded = isOnSleight || isOnFloor;

        if (isOnSleight && !isOnFloor)
        {
            playerTransform.parent = sleighTransform;
            sleighIsParent = true;
            info.enabled = GlobalGameState.DogsAlive > 0;
            cameraFollow.playerScreenXmin = 0.2f;
            cameraFollow.PlayerScreenXmax = 0.2f;
        }
        else if (isOnFloor && !isOnSleight)
        {
            playerTransform.parent = gameTransform;
            sleighIsParent = false;
            sleighTargetVelocity = 0;
            info.enabled = false;
            cameraFollow.playerScreenXmin = 0.2f;
            cameraFollow.PlayerScreenXmax = 0.8f;
        }

        if (GetKeyDown(interactionKey))
        {
            var dieScript = dieList.FirstOrDefault(d => d.Killable && !d.Dead);

            if (dieScript != null)
            {
                dieScript.Kill();
            }
        }

        if (sleighIsParent && GetKeyDown(interactionKey))
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

    private bool GetKey(KeyCode key)
    {
        if (playerDead)
            return false;

        return Input.GetKey(key);
    }

    private bool GetKeyDown(KeyCode key)
    {
        if (playerDead)
            return false;

        return Input.GetKeyDown(key);
    }

    private float GetAxisInput(string axis)
    {
        if (playerDead)
            return 0f;

        return Input.GetAxis(axis);
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