using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public Sprite[] colors;
    public int type;

	// Use this for initialization
	void Start () {
        // Random Index
        //int i = Random.Range(0, colors.Length);
        int i = Random.Range(0, 3);

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();

        /*if (i == 0) renderer.color = new Color(255f, 0f, 0f, 1f); // Set to opaque black
        if (i == 1) renderer.color = new Color(255f, 255f, 0f, 1f); // Set to opaque black
        if (i == 2) renderer.color = new Color(0f, 100f, 255f, 1f); // Set to opaque black
        */
        // (SpriteRenderer)GetComponent<SpriteRenderer>().color = Color.green;

        // 
        //renderer.sprite = colors[i];

    }

    // Update is called once per frame
    void Update () {
	
	}
}
