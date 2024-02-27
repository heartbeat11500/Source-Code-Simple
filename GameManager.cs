using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WaveStage
{
    public int maxEnemy;
    public GameObject prefab;
}

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public int score = 0;
    public TMP_Text scoreText;

    [Header("Stage")]
    private int stage = 0;
    public List<WaveStage> waveStages = new List<WaveStage>();
    public Slider wageProgress;

    [Header("Enemy")]
    public GameObject enemyPrefab;
    public List<Transform> enemyPoints = new List<Transform>();
    public int enemyMax = 150;
    public List<GameObject> enemies = new List<GameObject>();
    private float killAmount = 0;

    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        RunStage();
    }

    public void AddScore(int score)
    {
        this.score += score;
        killAmount++;
        scoreText.text = this.score.ToString();
        //playerController.IncreseDamage(0.1f);
        float progress = killAmount / enemyMax;
        wageProgress.value = progress;
        ScoreManager.instance.AddPoint();
        if (enemyMax <= killAmount)
        {
            stage++;
            Invoke("RunStage", 3);
        }

    }

    private void RunStage()
    {
        killAmount = 0;
        enemies.Clear();
        wageProgress.value = 0;
        if (waveStages.Count > stage)
        {
            enemyPrefab = waveStages[stage].prefab;
            enemyMax = waveStages[stage].maxEnemy;
            StartCoroutine(SpawnEnemy());
        }
        else
        {
            var enemy = Instantiate(bossPrefab.GetComponent<EnemyController>(), bossSpawnPoint.position, Quaternion.identity, null);
            enemy.player = playerController.transform;
            enemies.Add(enemy.gameObject);

            enemy.GetComponent<EnemyController>().OnDeath += LoadNextScene;
        }
    }

    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < enemyMax; i++)
        {
            var rd = Random.Range(0, enemyPoints.Count);
            var enemy = Instantiate(enemyPrefab.GetComponent<EnemyController>(), enemyPoints[rd].position, Quaternion.identity, null);
            enemy.player = playerController.transform;
            enemies.Add(enemy.gameObject);
            yield return new WaitForSeconds(0.2f);
        }
    }
    void Update()
    {
        scoreText.text = " " + score;
        PlayerPrefs.SetInt(" ", score);
    }

    // เมื่อ bossPrefab ถูกทำลาย เรียกใช้ฟังก์ชันนี้เพื่อโหลด Scene ต่อไป
    private void LoadNextScene()
    {
        SceneManager.LoadScene("CutScene3");
    }
}