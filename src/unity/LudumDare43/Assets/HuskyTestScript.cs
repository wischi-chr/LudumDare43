using UnityEngine;

public class HuskyTestScript : MonoBehaviour
{
    public int mutliSpeed = 40;
    public Animator animator;
    public SpriteRenderer SpriteRenderer;

    private Rigidbody2D playerRigid;

    // Use this for initialization
    void Start()
    {
        playerRigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var speed = playerRigid.velocity.x;

        animator.SetFloat("Speed", Mathf.Abs(speed));


        if (speed < -0.01 && !SpriteRenderer.flipX)
        {
            SpriteRenderer.flipX = true;
        }
        else if (speed > 0.01 && SpriteRenderer.flipX)
        {
            SpriteRenderer.flipX = false;
        }
    }
}
