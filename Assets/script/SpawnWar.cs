using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnWarrior : MonoBehaviour
{
    [Header("спавн")]
    public GameObject catPrefab;
    public Transform startPoint;
    public int maxCats = 14;

    [Header("tаймер")]
    public Image image;
    public Button spawnButton;
    public float cooldownTime = 2f;

    [Header("рыба")]
    public int catCost = 5;            
    public TextMeshProUGUI fishText;   
    public static int countWarrior = 0;
    private float timer = 0f;
    private bool isCooldown = false;

    void Start()
    {
        if (image != null)
            image.fillAmount = 1f;

        UpdateFishUI();
    }

    void Update()
    {
        if (isCooldown)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer / cooldownTime;

            if (timer >= cooldownTime)
            {
                isCooldown = false;
                image.fillAmount = 1f;

                if (countWarrior < maxCats && spawnButton != null)
                    spawnButton.interactable = true;
            }
        }
    }

    public void Spawn()
    {
        if (isCooldown || countWarrior >= maxCats || Fish.totalFish < catCost) return;
        Fish.totalFish -= catCost;
        UpdateFishUI();

        Vector3 origin = startPoint != null ? startPoint.position : transform.position;
        int xIndex = countWarrior % 5;
        int yIndex = countWarrior / 5;
        Vector3 pos = origin + new Vector3(xIndex * 1f, -yIndex * 0.6f, 0f);

        Instantiate(catPrefab, pos, Quaternion.identity);
        countWarrior++;

        if (countWarrior >= maxCats)
        {
            if (spawnButton != null) spawnButton.interactable = false;
            return;
        }

        isCooldown = true;
        timer = 0f;
        if (image != null) image.fillAmount = 0f;
        if (spawnButton != null) spawnButton.interactable = false;
    }

    private void UpdateFishUI()
    {
        if (fishText != null)
            fishText.text = $"{Fish.totalFish}";
    }
}