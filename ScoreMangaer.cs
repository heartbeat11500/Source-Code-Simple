using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreMangaer : MonoBehaviour
{
    public static ScoreMangaer instance;

    public TMP_Text  scoreText;
    public InputField valueText;
    [SerializeField]  private float fadeOutTime = 3f;

    static int score = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreText.text = "Score : " + score.ToString();
    }


    public void AddPoint()
    {
        score += 1;
        scoreText.text = "Score : " + score.ToString();
    }

    public void RemovePoint()
    {
        score -= 0;
        scoreText.text = "Score : " + score.ToString();
    }

    void Update ()
{
    scoreText.text = "Score: " + score;
	PlayerPrefs.SetInt ("Score", score);
}
public async void OnClick()
{
    int secondToMilisecond = (int)(fadeOutTime * 1000f);
    await System.Threading.Tasks.Task.Delay(secondToMilisecond);
     score = 0;
}
}
