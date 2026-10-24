using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fish : MonoBehaviour
{
    [Header("спавнер")]
    public CatSpawner spawner;

    [Header("таймер")]
    public Image fishTimerImage;    
    public float fishInterval = 5f; 
    public TextMeshProUGUI fishText;

    public static int totalFish = 0;
    private float timer = 0f;

    void Start()
    {
        if (fishTimerImage != null)
            fishTimerImage.fillAmount = 0f;

        UpdateUI();
    }

    void Update()
    {
        if (spawner == null || spawner.catCount == 0) return;

        timer += Time.deltaTime;

        if (fishTimerImage != null)
            fishTimerImage.fillAmount = timer / fishInterval;

        if (timer >= fishInterval)
        {
            timer = 0f;
            if (fishTimerImage != null)
                fishTimerImage.fillAmount = 0f;

            totalFish += spawner.catCount * 3;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (fishText != null)
            fishText.text = $"{totalFish}";
    }
}
