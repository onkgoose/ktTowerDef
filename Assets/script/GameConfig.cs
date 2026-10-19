using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Village/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("стартовые значения")]
    public int startingFish = 30;
    public int startingKittens = 1;
    public int startingWarriors = 1;

    [Header("производство и потребление рыбы")]
    [Tooltip("интервал сбора рыбы крестьянами (в сек)")]
    public float fishHarvestInterval = 3f;
    [Tooltip("сколько рыбы приносит 1 котенок за сбор")]
    public int fishPerKitten = 2;

    [Tooltip("интервал потребления рыбы воинами (в сек)")]
    public float fishConsumptionInterval = 4f;
    [Tooltip("сколько рыбы съедает 1 воин за цикл")]
    public int fishPerWarrior = 1;

    [Header("найм юнитов")]
    public int kittenCost = 10;
    public float kittenTrainTime = 3f;

    public int warriorCost = 25;
    public float warriorTrainTime = 5f;

    [Header("враги")]
    [Tooltip("время между нападениями")]
    public float attackInterval = 25f;
    [Tooltip("базовое число врагов в 1-й волне")]
    public int initialEnemies = 2;
    [Tooltip("на сколько увеличивается следующая волна")]
    public int enemyGrowthPerWave = 2;
    [Tooltip("сколько волн нужно выдержать для победы")]
    public int totalWavesToWin = 10;

    [Header("бой")]
    [Tooltip("сколько воинов нужно на 1 врага")]
    public int warriorsPerEnemy = 1;
    [Tooltip("сколько крестьян нужно на 1 врага, если нет воинов")]
    public int kittensPerEnemy = 3;

    [Header("случайные события")]
    [Tooltip("интервал проверки случайного события")]
    public float eventInterval = 20f;
    [Range(0f, 1f)]
    [Tooltip("шанс наступления события при проверке")]
    public float eventChance = 0.35f;
}
