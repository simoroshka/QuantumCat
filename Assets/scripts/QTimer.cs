using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QTimer : MonoBehaviour
{
    public bool catAlive = true;
    public float time;
    public bool qtime = false;

    private float currentTime = 0;
    private TextMesh timeText;

    private float probability = 1.0f;
    private float timeTick = 0;
    private float startTime;

    // Use this for initialization
    void Start()
    {
       // GetComponent<RectTransform>().position = new Vector3((float)Screen.width * 0.97f, (float)Screen.height / 2, 0f);
        timeText = GetComponent<TextMesh>();
  
     //   GameObject.Find("qtime").GetComponent<AudioSource>().Play();
    }

   

    // Update is called once per frame
    void Update()
    {
        //check winning conditions
        if (Grid.isWinCondition())
        {

            if (catAlive)
            {
                //win! cat is alive!
                Application.LoadLevel("win");
            }
            else
            {
                //lose! cat is dead...
                Application.LoadLevel("lose");
            }
        }
        //chack losing condition
        if (probability < 0.01f)
        {
            //lose! cat is too probably dead...
            Application.LoadLevel("lose");
        }

        if (qtime)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeTick + 1)
            {
                timeTick = currentTime;
                float n = Random.Range(0.0f, 1.0f);
                probability = Mathf.Exp(-4.6f * currentTime / 90);
                if (n > probability)
                {
                    //cat is dead
                    catAlive = false;
                }
                //show probability

                timeText.text = (probability * 100).ToString("F1") + "%";
            }
        }
        
        


        
        /*
        int minutes = (int)(currentTime / 60);
        int seconds = (int)(currentTime % 60);

        if (currentTime < time)
        {
            //display the timer
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            //time is up
            //GAME OVER, open the box


        }*/
    }
}
