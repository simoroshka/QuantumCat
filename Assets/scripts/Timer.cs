using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public float time;

    private TextMesh timeText;
    public GameObject QuantumTimer;
    public GameObject QuantumAudio;

    public GameObject particlePrefab;

    // Use this for initialization
    void Start()
    {
        timeText = GetComponent<TextMesh>();
        //GetComponent<RectTransform>().position = new Vector3((float)Screen.width * 0.22f, (float)Screen.height / 2, 0f);
           
        //  QTimer.SetActive(false);
        Grid.initialize(particlePrefab);
        // GetComponent<AudioSource>().Stop();
      //  GameObject.Find("qtime").GetComponent<AudioSource>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //check winning conditions
        if (Grid.isWinCondition() && time > 0) {


            //win! cat is alive!
            Application.LoadLevel("win");
        }


        time -= Time.deltaTime;

        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);

        if (time > 0)
        {
            //display the timer
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            //fade the audio out if we are approaching the end
            if (time < 3)
            {
                GetComponent<AudioSource>().volume = 1 * time / 3;
            }
        }
        else
        {
            //time is up
            //START QUANTUM TIMER

            //QTimer.SetActive(true);
            QuantumTimer.GetComponent<QTimer>().qtime = true;
            //  GameObject.Find("bgmusic").GetComponent<AudioSource>().Stop();
            QuantumAudio.SetActive(true);
            GetComponent<AudioSource>().Stop();

        }
    }
}
