using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        float speed = 10;
        float moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float moveVertical = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        var pos = gameObject.transform.position;
        pos.x += moveHorizontal;
        gameObject.transform.position = pos;
    }
}
