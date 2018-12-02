using System;
using System.Collections.Generic;
using UnityEngine;

public class SleighMovementController : MonoBehaviour
{
    public float targetVelocity = 0;
    public float velocity;

    public float acceleration = 1f;
    public float deceleration = 10f;
    public bool IsEnabled = false;

    Transform sleighTransform;
    List<GameObject> huskies;
    List<Animator> huskyAnimators;

    string[] dogNames = { "Happy", "Rudolph", "Pavlov", "Hachiko" };

    // Use this for initialization
    void Start()
    {
        FindHuskies(gameObject);
        sleighTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled)
            return;

        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        //friction for targetVelocity
        //var deltaV = targetVelocity * velocityFriction * Time.deltaTime;
        //targetVelocity -= deltaV;

        if (velocity < targetVelocity)
            velocity += acceleration * Time.deltaTime;
        else
            velocity -= deceleration * Time.deltaTime;

        if ((velocity < 0.5f && targetVelocity < velocity) || velocity < 0)
            velocity = 0;

        var locPos = sleighTransform.localPosition;
        locPos.x += velocity * Time.deltaTime;
        sleighTransform.localPosition = locPos;
        SetHuskeySpeed(velocity);
    }

    void SetHuskeySpeed(float speed)
    {
        foreach (var ani in huskyAnimators)
        {
            var abs = Mathf.Abs(speed);
            ani.SetFloat("Speed", abs);
            ani.speed = abs * 0.1f;
        }
    }

    void FindHuskies(GameObject sleigh)
    {
        huskies = new List<GameObject>();
        huskyAnimators = new List<Animator>();
        var cnt = 0;

        foreach (Transform child in transform)
        {
            if (Array.IndexOf(dogNames, child.gameObject.name) > -1)
            {
                huskies.Add(child.gameObject);
                huskyAnimators.Add(child.gameObject.GetComponent<Animator>());
                cnt++;
            }
        }

        Debug.Log("Huskies found: " + cnt);
    }
}
