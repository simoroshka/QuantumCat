using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject[] groups;
   

    public void spawnNext()
    {
        // Random Index
        int i = Random.Range(0, groups.Length);
        // random orientation
        int o = Random.Range(0, 4);

        // Spawn Group at current Position
        Instantiate(groups[i],
                    transform.position,
                    Quaternion.identity);
       
    }

    // Use this for initialization
    void Start () {
        spawnNext();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
