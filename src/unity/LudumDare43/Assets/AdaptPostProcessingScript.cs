using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class AdaptPostProcessingScript : MonoBehaviour {
    private PostProcessingProfile pp;

    // Use this for initialization
    void Start () {
        pp = GetComponent<PostProcessingBehaviour>().profile;

        
    }
	
	// Update is called once per frame
	void Update () {
        var settings = pp.vignette.settings;
        settings.roundness = 1f - GlobalGameState.Food;
        pp.vignette.settings = settings;

    }
}
