using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    public GameObject ObjectToFollow;

    public float playerScreenXmin = 0.2f;
    public float PlayerScreenXmax = 0.8f;

    private Camera cam;

    private float cameraXtarget;
    private float cameraSpeedX;
    private float lastObjPos = float.NaN;

    // Use this for initialization
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentObjectX = ObjectToFollow.transform.position.x;

        if (float.IsNaN(lastObjPos))
            lastObjPos = currentObjectX;

        var objVel = (currentObjectX - lastObjPos) / Time.deltaTime;
        lastObjPos = currentObjectX;

        var playerScreenX = GetViewPortX(currentObjectX);

        float diff = 0;

        if (playerScreenX > PlayerScreenXmax)
        {
            diff = playerScreenX - PlayerScreenXmax;
        }

        else if (playerScreenX < playerScreenXmin)
        {
            diff = playerScreenX - playerScreenXmin;
        }

        var diffAbs = Mathf.Abs(diff);
        if (diffAbs > 0)
        {
            cameraXtarget = GetWorldX(0.5f + diff);

            var time = 1.0f / diffAbs * 0.01f;

            var camPos = cam.transform.position;
            camPos.x = Mathf.SmoothDamp(camPos.x, cameraXtarget, ref cameraSpeedX, time, Mathf.Abs(objVel) + 10f);
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