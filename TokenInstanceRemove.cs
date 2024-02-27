using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenInstanceRemove : MonoBehaviour
{
    public AudioSource wrong;
    void OnTriggerEnter(Collider other)
    {
        wrong.Play();
        ScoreMangaer.instance.RemovePoint();
    }
}
