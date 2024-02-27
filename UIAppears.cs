using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIAppears : MonoBehaviour
{
    [SerializeField]  private string targetTag = "Player";
    [SerializeField]  private float fadeOutTime = 3f;
    public GameObject popUp;
    
    void Start()
    {
        popUp.SetActive (false);
    }


    private void ActiveUI(bool active)
    {
        popUp.SetActive(active);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            ActiveUI(true);
        }
    }

    
    void OnTriggerExit (Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            ActivateAsync(false);
        }
    }

    private async void ActivateAsync(bool active)
    {
        int secondToMilisecond = (int)(fadeOutTime * 1000f);
        await System.Threading.Tasks.Task.Delay(secondToMilisecond);
        ActiveUI(active);
    }
    
}
