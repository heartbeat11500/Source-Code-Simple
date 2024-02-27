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
            damage++; // ��������������¢�鹷ء���駷�誹�Ѻ Obstacle
            if (damage <= hearts.Length)
            {
                Destroy(hearts[damage - 1]); // ����� heart ����ӴѺ��������������
            }
            if (damage >= hearts.Length)
            {
                SceneManager.LoadScene("GAMEOVER"); // ��Ҥ�����������ҡ����������ҡѺ�ӹǹ�ͧ heart �ʴ���� Player ��ͧ��è���
            }
        }
    }
}