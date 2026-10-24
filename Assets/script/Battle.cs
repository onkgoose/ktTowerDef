using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Связи со спавнерами")]
    public SpawnWarrior warriorSpawner;
    public CatSpawner workerSpawner;

    [Header("Префаб врага и спавн")]
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;

    [Header("Таймеры")]
    public Image timerImage;
    public float waveDuration = 10f;    
    public float enemyLifeTime = 4f;       

    [Header("Экраны концовки (UI Panels)")]
    public GameObject winScreen;
    public GameObject gameOverScreen;

    private int currentCycle = 1;
    private int enemyCount = 1;
    private float timer = 0f;
    private bool hasStarted = false;
    private bool gameEnded = false;
    private bool enemiesClearedThisWave = false;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        if (timerImage != null)
            timerImage.fillAmount = 1f;

        if (winScreen != null) winScreen.SetActive(false);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
    }

    void Update()
    {
        if (gameEnded) return;

        if (!hasStarted)
        {
            if (SpawnWarrior.countWarrior > 0)
            {
                hasStarted = true;
                StartNewWave();
            }
            else
            {
                return;
            }
        }

        timer += Time.deltaTime;

        if (timerImage != null)
            timerImage.fillAmount = Mathf.Clamp01(1f - (timer / waveDuration));

        if (!enemiesClearedThisWave && timer >= enemyLifeTime)
        {
            ClearOldEnemies();
            enemiesClearedThisWave = true;
        }

        if (timer >= waveDuration)
        {
            EndCycleAndBattle();
        }
    }

    void StartNewWave()
    {
        timer = 0f;
        enemiesClearedThisWave = false;

        if (timerImage != null)
            timerImage.fillAmount = 1f;

        ClearOldEnemies();
        SpawnEnemies();
    }

    void EndCycleAndBattle()
    {
        ClearOldEnemies();

        int warriors = SpawnWarrior.countWarrior;
        int workers = workerSpawner != null ? workerSpawner.catCount : 0;

        bool isVictory = false;
        if (warriors > 0)
        {
            if (warriors != enemyCount)
                isVictory = true;
        }
        else
        {
            if (workers != (enemyCount * 3))
                isVictory = true;
        }

        if (!isVictory)
        {
            gameEnded = true;
            ShowGameOver();
            return;
        }

        if (currentCycle >= 10)
        {
            gameEnded = true;
            ShowWin();
            return;
        }

        currentCycle++;

        if (currentCycle >= 5)
            enemyCount += 5;
        else
            enemyCount += 1;

        StartNewWave();
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null) return;

        Vector3 basePos = enemySpawnPoint != null ? enemySpawnPoint.position : transform.position;

        for (int i = 0; i < enemyCount; i++)
        {
            int yIndex = i % 7;
            int xIndex = i / 7;

            Vector3 spawnPos = basePos + new Vector3(-xIndex * 0.8f, -yIndex * 0.3f, 0f);
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    void ClearOldEnemies()
    {
        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            if (spawnedEnemies[i] != null)
                Destroy(spawnedEnemies[i]);
        }
        spawnedEnemies.Clear();
    }

    void ShowWin()
    {
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            winScreen.transform.SetAsLastSibling(); 
        }
    }

    void ShowGameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            gameOverScreen.transform.SetAsLastSibling(); 
        }
    }
}