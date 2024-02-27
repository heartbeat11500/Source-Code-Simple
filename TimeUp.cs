using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeUp : MonoBehaviour
{
    public TMP_Text timerText;
    float minutes = 1;
    public float seconds = 0;
    private float startTime;
    public float timeLeft;
    void Start()
    {
        startTime = Time.time;
    }
    void Update()
    {
        if (minutes > 0 && Mathf.Approximately(seconds, 0))
        {
            minutes--;
            seconds = 120;
            timerText.text = seconds.ToString("0 s");
            Debug.Log(minutes + " " + seconds);
        }
        else if (seconds > 0)
        {
            seconds -= Time.deltaTime;
            timerText.text = seconds.ToString("0 s");
            Debug.Log(minutes + " " + seconds);
        }
        else if (Mathf.Approximately(minutes, 0) && seconds <= 0)
        {
            SceneManager.LoadScene("TotalScore");
            Debug.Log("TotalScore");
        }
    }

    public void AddTime(float time)
    {
        seconds += time;
    }

    public void RemoveTime(float time) 
    {
        seconds -= time;
    }
}
