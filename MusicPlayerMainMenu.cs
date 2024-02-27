using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayerMainMenu : MonoBehaviour
{
public static MusicPlayerMainMenu musicplay;
public GameObject music;
public AudioClip click;
public AudioSource sfxSource;
public List<AudioClip> sounds = new List<AudioClip>();


void Awake()
{
    if (musicplay == null)
    {
        DontDestroyOnLoad(gameObject);
        musicplay = this;
    }
    else if (musicplay != this)
    {
        Destroy(gameObject);
    }
}

void Update()
{
    Scene currentScene = SceneManager.GetActiveScene();

    if (currentScene.name == "CutScene1")
    {
        // Stops playing music in level 1 scene
        Destroy(gameObject);
    }
}

public void SoundClick()
    {
        sfxSource.clip = click;
        sfxSource.Play();
    }
}
