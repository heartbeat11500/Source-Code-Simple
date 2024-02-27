using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalForEnd : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<AnimationandMovementController>(out var player))
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        SceneManager.LoadScene("CutScene3");
    }
}
