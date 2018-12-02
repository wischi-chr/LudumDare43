using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudDistanceDisplay : MonoBehaviour {

    Text hudDistanceText;

    // Use this for initialization
    void Start()
    {
        hudDistanceText = GameObject.Find("hud-distance-text").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hudDistanceText.text = string.Format("{0:# ##0.0}", GlobalGameState.Distance);
    }
}
