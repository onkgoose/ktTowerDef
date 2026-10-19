using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameConfig config;
    [SerializeField] private UIManager uiManager;
    public int Fish { get; private set; }
    public int Kittens { get; private set; }
    public int Warriors { get; private set; }

    public int CurrentWave { get; private set; } = 1;
    public int NextWaveEnemies { get; private set; }
    public float TimeUntilAttack { get; private set; }
    private int kittenQueue = 0;
    private int warriorQueue = 0;
    private bool isHiringKitten = false;
    private bool isHiringWarrior = false;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 0f;
        uiManager.ShowStartScreen();
    }
    public void StartGame()
    {
        Fish = config.startingFish;
        Kittens = config.startingKittens;
        Warriors = config.startingWarriors;
        CurrentWave = 1;
        CalculateNextWave();

        isGameOver = false;
        Time.timeScale = 1f;

        uiManager.ShowGameScreen();
        UpdateUI();

        StartCoroutine(HarvestRoutine());
        StartCoroutine(ConsumptionRoutine());
        StartCoroutine(AttackCountdownRoutine());
        StartCoroutine(RandomEventsRoutine());
    }

    private void Update()
    {
        if (isGameOver || Time.timeScale == 0f) return;
        uiManager.SetKittenButtonInteractable(Fish >= config.kittenCost);
        uiManager.SetWarriorButtonInteractable(Fish >= config.warriorCost);
    }

    // fish add

    private IEnumerator HarvestRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(config.fishHarvestInterval);

            int harvested = Kittens * config.fishPerKitten;
            Fish += harvested;
            UpdateUI();
        }
    }

    private IEnumerator ConsumptionRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(config.fishConsumptionInterval);

            int fishNeeded = Warriors * config.fishPerWarrior;

            if (Fish >= fishNeeded)
            {
                Fish -= fishNeeded;
            }
            else
            {
                int fedWarriors = config.fishPerWarrior > 0 ? Fish / config.fishPerWarrior : Warriors;
                int starved = Warriors - fedWarriors;
                Fish = 0;
                Warriors = Mathf.Max(0, fedWarriors);
                uiManager.ShowNotification($"units is starve! {starved} warriors is dead.");
            }

            UpdateUI();
        }
    }

    // unit hire

    public void OnHireKittenClicked()
    {
        if (Fish >= config.kittenCost)
        {
            Fish -= config.kittenCost;
            kittenQueue++;
            UpdateUI();

            if (!isHiringKitten)
            {
                StartCoroutine(HireKittenRoutine());
            }
        }
    }

    private IEnumerator HireKittenRoutine()
    {
        isHiringKitten = true;
        while (kittenQueue > 0)
        {
            yield return new WaitForSeconds(config.kittenTrainTime);
            Kittens++;
            kittenQueue--;
            UpdateUI();
        }
        isHiringKitten = false;
    }

    public void OnHireWarriorClicked()
    {
        if (Fish >= config.warriorCost)
        {
            Fish -= config.warriorCost;
            warriorQueue++;
            UpdateUI();

            if (!isHiringWarrior)
            {
                StartCoroutine(HireWarriorRoutine());
            }
        }
    }

    private IEnumerator HireWarriorRoutine()
    {
        isHiringWarrior = true;
        while (warriorQueue > 0)
        {
            yield return new WaitForSeconds(config.warriorTrainTime);
            Warriors++;
            warriorQueue--;
            UpdateUI();
        }
        isHiringWarrior = false;
    }
    //boy
    private void CalculateNextWave()
    {
        NextWaveEnemies = config.initialEnemies + (CurrentWave - 1) * config.enemyGrowthPerWave;
        TimeUntilAttack = config.attackInterval;
    }

    private IEnumerator AttackCountdownRoutine()
    {
        while (!isGameOver)
        {
            while (TimeUntilAttack > 0)
            {
                yield return new WaitForSeconds(1f);
                TimeUntilAttack -= 1f;
                uiManager.UpdateAttackTimer(TimeUntilAttack);
            }

            ResolveBattle();

            if (isGameOver) yield break;

            if (CurrentWave >= config.totalWavesToWin)
            {
                TriggerVictory();
                yield break;
            }

            CurrentWave++;
            CalculateNextWave();
            UpdateUI();
        }
    }

    private void ResolveBattle()
    {
        int enemies = NextWaveEnemies;
        int initialWarriors = Warriors;
        int initialKittens = Kittens;

        int warriorsSacrificed = Mathf.Min(Warriors, enemies * config.warriorsPerEnemy);
        Warriors -= warriorsSacrificed;
        enemies -= warriorsSacrificed / config.warriorsPerEnemy;

        int kittensSacrificed = 0;
        if (enemies > 0)
        {
            int kittensNeeded = enemies * config.kittensPerEnemy;
            kittensSacrificed = Mathf.Min(Kittens, kittensNeeded);
            Kittens -= kittensSacrificed;
            enemies -= kittensSacrificed / config.kittensPerEnemy;
        }
        if (enemies > 0 || (Kittens <= 0 && Warriors <= 0))
        {
            TriggerDefeat();
            return;
        }

        uiManager.ShowNotification($"wave is completed: {warriorsSacrificed} warriors, {kittensSacrificed} cats.");
        UpdateUI();
    }

    // random event

    private IEnumerator RandomEventsRoutine()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(config.eventInterval);

            if (Random.value <= config.eventChance)
            {
                ApplyRandomEvent();
            }
        }
    }

    private void ApplyRandomEvent()
    {
        int eventType = Random.Range(0, 3);
        switch (eventType)
        {
            case 0: // (теряем рыбу)
                int lostFish = Mathf.Max(5, Fish / 3);
                Fish = Mathf.Max(0, Fish - lostFish);
                uiManager.ShowNotification($"tired kittens have eaten {lostFish} of fish!");
                break;
            case 1: //(болеют котята)
                if (Kittens > 1)
                {
                    int lostKittens = Mathf.Max(1, Kittens / 3);
                    Kittens -= lostKittens;
                    uiManager.ShowNotification($"scary disease killed {lostKittens} cats...");
                }
                break;
            case 2: // (бонус)
                int bonusFish = 15 + CurrentWave * 5;
                Fish += bonusFish;
                uiManager.ShowNotification($"a school of cod near! +{bonusFish} fishes!");
                break;
        }
        UpdateUI();
    }

    private void TriggerVictory()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        uiManager.ShowVictoryScreen(CurrentWave);
    }

    private void TriggerDefeat()
    {
        isGameOver = true;
        Time.timeScale = 0f;
        uiManager.ShowDefeatScreen(CurrentWave - 1);
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        isHiringKitten = false;
        isHiringWarrior = false;
        kittenQueue = 0;
        warriorQueue = 0;
        StartGame();
    }
    private void UpdateUI()
    {
        uiManager.UpdateResourceLabels(Fish, Kittens, Warriors, kittenQueue, warriorQueue);
        uiManager.UpdateWaveInfo(CurrentWave, NextWaveEnemies);
    }
}
