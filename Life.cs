using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Life : MonoBehaviour
{
    public GameObject[] hearts;
    private int damage = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("damage"))
        {
            damage++; // เพิ่มความเสียหายขึ้นทุกครั้งที่ชนกับ Obstacle
            if (damage <= hearts.Length)
            {
                Destroy(hearts[damage - 1]); // ทำลาย heart ตามลำดับตามความเสียหาย
            }
            if (damage >= hearts.Length)
            {
                SceneManager.LoadScene("GAMEOVER"); // ถ้าความเสียหายมากกว่าหรือเท่ากับจำนวนของ heart แสดงว่า Player ต้องการจบเกม
            }
        }
    }
}