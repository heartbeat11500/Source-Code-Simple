using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TokenInstance : MonoBehaviour
{
    public AudioSource correct;
    
    void  OnTriggerEnter(Collider other)
    {
        correct.Play();
        ScoreMangaer.instance.AddPoint();
        
    }
}
