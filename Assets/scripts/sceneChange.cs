using UnityEngine;
using System.Collections;

public class sceneChange : MonoBehaviour {

    public string nextScene;
    public GameObject[] slides;
    private int currentSlide = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        { 

                if (currentSlide == slides.Length - 1)
                {
                
                    if (nextScene.Equals("exit"))
                    {
                    Debug.Log("QUIT");
                    Application.Quit();
                    }
                    else
                    {
                        Application.LoadLevel(nextScene);
                    }
                    
                
            }
            else if (slides.Length != 0)
            {
                currentSlide++;
                slides[currentSlide-1].SetActive(false);
                slides[currentSlide].SetActive(true);
                
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


    }
}
