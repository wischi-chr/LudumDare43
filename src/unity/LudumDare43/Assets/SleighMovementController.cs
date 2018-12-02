using System;
using System.Collections.Generic;
using UnityEngine;

public class SleighMovementController : MonoBehaviour
{
    public float targetVelocity = 0;
    public float velocityFriction = 0.9f;
    public bool holdSpeed = false;
    public bool IsEnabled = false;

    float constantAcceleration = 0.1f;
    float velocity;
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
        if (!holdSpeed)
        {
            //friction for targetVelocity
            var deltaV = targetVelocity * velocityFriction * Time.deltaTime;
            targetVelocity -= deltaV;
        }

        if (targetVelocity > velocity)
            velocity -= constantAcceleration;
        else
            velocity += constantAcceleration;

        var locPos = sleighTransform.localPosition;
        locPos.x += targetVelocity * Time.deltaTime;
        sleighTransform.localPosition = locPos;

        SetHuskeySpeed(targetVelocity > 2.5f ? 1 : 0);
    }

    void SetHuskeySpeed(float speed)
    {
        foreach (var ani in huskyAnimators)
        {
            var abs = Mathf.Abs(speed);
            ani.SetFloat("Speed", abs);
            Debug.Log("Set Speed: " + abs);
            if(speed > 0)
            {
                ani.speed = abs * 0.5f;
            }
            ani.speed = 1;
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
