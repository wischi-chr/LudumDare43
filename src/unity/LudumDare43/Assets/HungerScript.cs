using Assets;
using UnityEngine;

public class HungerScript : MonoBehaviour
{
    public float IdleHungerPointsPerSecond = 0.01f;
    public float VelocityXPointFactor = 0.01f;
    public float VelocityYPointFactor = 0.01f;

    private Rigidbody2D playerRigidbody;

    // Use this for initialization
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var hungerPoints = Time.deltaTime *
        (
            IdleHungerPointsPerSecond
            + Mathf.Abs(playerRigidbody.velocity.x) * VelocityXPointFactor
            + Mathf.Abs(playerRigidbody.velocity.y) * VelocityYPointFactor
        );

        GlobalGameState.Food -= hungerPoints;
    }
}
