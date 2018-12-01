using UnityEngine;

public class ParalaxBG : MonoBehaviour
{

    public int Depth;
    public float Speed;
    public float Offset;
    public float Scale;

    public Sprite Sprite;

    private Camera cameraToFollow;
    public Vector3 spriteSize;

    private GameObject[] panels = new GameObject[3];

    // Use this for initialization
    void Start()
    {
        cameraToFollow = Camera.main;
        spriteSize = Sprite.bounds.size;

        for (int i = 0; i < panels.Length; i++)
        {
            var panel = new GameObject();
            var backgroundSprite = panel.AddComponent<SpriteRenderer>();
            backgroundSprite.sprite = Sprite;
            panels[i] = panel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var pos = cameraToFollow.gameObject.transform.position;
        pos.z += Depth;

        //This overlap is needed to prevent glitching on stitched sprite tiles
        var overlap = 0.01f;
        var dist = Sprite.bounds.size.x;
        var Xparalax = pos.x - ((Speed * pos.x / dist) % 1) * dist + dist / 2;

        pos.x = Xparalax;
        panels[1].gameObject.transform.position = pos;

        pos.x = Xparalax - (dist - overlap);
        panels[0].gameObject.transform.position = pos;

        pos.x = Xparalax + (dist - overlap);
        panels[2].gameObject.transform.position = pos;

    }
}
