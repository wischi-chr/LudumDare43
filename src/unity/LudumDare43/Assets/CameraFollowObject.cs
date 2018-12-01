using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{

    public GameObject ObjectToFollow;
    public bool LockX, LockY;
    public bool DebugInformation = true;
    public Vector3 DestinationDifference = Vector3.zero;
    public float Offset = 2f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var destination = new Vector3(LockX ? 0 : ObjectToFollow.transform.position.x, LockY ? 0 : ObjectToFollow.transform.position.y, transform.position.z);
        destination += DestinationDifference;
        float distance = Vector3.Distance(transform.position, destination);

        if (distance > Offset)
        {

            distance -= Offset;

            var heading = destination - transform.position;
            heading *= Time.deltaTime;
            heading *= distance * 2;

            transform.position = new Vector3(
                LockX ? transform.position.x : transform.position.x + heading.x,
                LockY ? transform.position.y : transform.position.y + heading.y,
                transform.position.z);
        }

        if (DebugInformation)
        {
            DrawDebugInformation();
        }
    }

    private void DrawDebugInformation()
    {
        var position = new Vector3(transform.position.x, transform.position.y, 0);
        var destination = new Vector3(LockX ? 0 : ObjectToFollow.transform.position.x, LockY ? 0 : ObjectToFollow.transform.position.y, 0);
        destination += DestinationDifference;

        Debug.DrawRay(position, destination - position, Color.black);
    }
}