using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject FollowTarget;

    public bool FollowX = true;

    private Camera camera;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(FollowX)
        {
            var p = camera.gameObject.transform.position;
            p.x = FollowTarget.transform.position.x;
            camera.gameObject.transform.position = p;
        }
    }
}
