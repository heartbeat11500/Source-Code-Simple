using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AddTime : MonoBehaviour
{
    [SerializeField] private string targetTag = "Untagged";
    public UnityEvent OnTrigger;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag(targetTag))
        {
            OnTrigger.Invoke();
        }
    }
}
