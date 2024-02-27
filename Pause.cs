using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject PausePanel;
    public GameObject TimeScore;
    public AudioSource audio;
    public AudioSource walk;
    public AudioSource run;

    public void PauseMenu()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        audio.Pause();
        TimeScore.SetActive(true);
        walk.mute = true;
        run.mute = true;
    }

    public void Continue()
    {
        PausePanel.SetActive(true);
        TimeScore.SetActive(true);
        Time.timeScale = 1;
        audio.Play();
        walk.mute = false;
        run.mute = false;

    }
}
   
