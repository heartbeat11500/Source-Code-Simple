using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int life = 3;

    //animation
    Animator animator;

    CharacterController controller;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public bool isEnemyTrigger = false;
    public float getDamage = 0;
    private float delayHurt = 1;

    public Transform respawnPoint;

    [Header("Bullet")]
    public float damage = 1f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float bulletForce = 30;
    public float reload = 0.1f;

    //Audio
    public AudioSource Gun;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(Shoot());
        //StartCoroutine(CheckHurt());
    }
    /* CheckHurt()
    {
        while (true)
        {
            if (isEnemyTrigger)
            {
                AddDamage(getDamage);
            }
            yield return new WaitForSeconds(delayHurt);
        }
    }*/


    IEnumerator Shoot()
    {
        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation, null);
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Rigidbody>().AddForce(shootPoint.forward * bulletForce, ForceMode.Impulse);
                Destroy(bullet, 3);
                Gun.Play();
            }
            yield return new WaitForSeconds(reload);
        }
    }

    void FixedUpdate()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        animator.SetBool("isWalking", Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlace = new Plane(Vector3.up, Vector3.zero);
        if (groundPlace.Raycast(cameraRay, out var rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

    }

    public float maxHp = 100;
    public float hp = 100;
    public Slider hpSlider;
    public void AddDamage(float damage)
    {
        hp -= damage;
        hpSlider.value = hp / maxHp;
    }

    /*public void IncreseDamage(float d)
    {
        damage += d;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other && other.CompareTag("Enemy"))
        {
            isEnemyTrigger = true;
            AddDamage(getDamage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
            isEnemyTrigger = false;
    }

    public void GameOver()
    {
        if (hp <= 0)
        {
            hpSlider.value = 0;
            SceneManager.LoadScene("SCORE");
            Debug.Log("SCORE");
        }
    }
}