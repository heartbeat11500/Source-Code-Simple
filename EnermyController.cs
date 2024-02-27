using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    public float damage = 5;
    public float hp = 10;
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

    }

    public void OnTriggerEnter(Collider other)
    { 
        if (other.tag == "Bullet")
        {
            hp -= other.GetComponent<Bullet>().damage;

            if (hp <= 0)
            {

                SoundDie.instance.PlayDie();
                Destroy(gameObject);
                GameManager.instance.AddScore(10);
                    
            }
        }
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().getDamage = damage;
        }
    }
    public event System.Action OnDeath;
    private void OnDestroy()
    {
        // เรียกเมื่อ bossPrefab ถูกทำลาย
        OnDeath?.Invoke();
    }
}