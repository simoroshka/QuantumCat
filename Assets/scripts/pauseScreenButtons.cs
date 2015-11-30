using UnityEngine;
using System.Collections;

public class pauseScreenButtons : MonoBehaviour {

    public bool yes;
    void onMouseDown()
    {
        if (yes)
        {
            Application.Quit();
        }
        else
        {

        }
    }
}
