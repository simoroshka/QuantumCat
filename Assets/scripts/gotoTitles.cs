using UnityEngine;
using System.Collections;

public class gotoTitles : MonoBehaviour {

    private AudioSource sound;
	// Use this for initialization
	void Start () {
        sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (!sound.isPlaying)
        {
            //start title screen

        }
	}
}
