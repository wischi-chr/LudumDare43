using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject ObjectToFollow;

    public float playerScreenXmin = 0.2f;
    public float PlayerScreenXmax = 0.8f;

    private Camera cam;
    private float lastObjectXPos;

    private float cameraXtarget;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentObjectX = ObjectToFollow.transform.position.x;
        var objectVelocity = (currentObjectX - lastObjectXPos) / Time.deltaTime;
        lastObjectXPos = currentObjectX;

        var futurePosition = currentObjectX + objectVelocity * 1f;

        Debug.Log("currentObjectX: " + currentObjectX);
        Debug.Log("objectVelocity: " + objectVelocity);
        Debug.Log("futurePosition: " + futurePosition);

        var playerScreenX = GetViewPortX(futurePosition);

        float diff = 0;

        if (playerScreenX > PlayerScreenXmax)
        {
            diff = playerScreenX - PlayerScreenXmax;
        }

        else if (playerScreenX < playerScreenXmin)
        {
            diff = playerScreenX - playerScreenXmin;
        }

        

        if (Mathf.Abs(diff) > 0)
        {
            cameraXtarget = GetWorldX(0.5f + diff);

            //smooth camtarget

            var camPos = cam.transform.position;

            var distX = cameraXtarget - camPos.x;
            var distAbs = Mathf.Abs(distX);

            var speedX = distAbs;
            speedX *= Mathf.Sign(distX);

            var distanceInTick = speedX * Time.deltaTime;
            camPos.x += distanceInTick;

            cam.transform.position = camPos;
        }
    }

    private float GetWorldX(float viewportX)
    {
        var point = new Vector3();
        point.x = viewportX;
        return cam.ViewportToWorldPoint(point).x;
    }

    private float GetViewPortX(float worldX)
    {
        var point = new Vector3();
        point.x = worldX;
        return cam.WorldToViewportPoint(point).x;
    }

    private void DrawDebugInformation()
    {

    }
}