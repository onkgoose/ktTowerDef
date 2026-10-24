using UnityEngine;
using UnityEngine.UI;

public class CatSpawner : MonoBehaviour
{
    [Header("спавн")]
    public GameObject catPrefab;
    public Transform startPoint;
    public int maxCats = 14;

    [Header("tаймер")]
    public Image image;  
    public Button spawnButton; 
    public float cooldownTime = 2f;

    public int catCount = 0;
    private float timer = 0f;
    private bool isCooldown = false;

    void Start()
    {
        if (image != null)
            image.fillAmount = 1f;
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

                if (catCount < maxCats && spawnButton != null)
                    spawnButton.interactable = true;
            }
        }
    }

    public void Spawn()
    {
        if (isCooldown || catCount >= maxCats) return;
        Vector3 origin = startPoint != null ? startPoint.position : transform.position;
        int xIndex = catCount % 7;
        int yIndex = catCount / 7;
        Vector3 pos = origin + new Vector3(xIndex * 1f, -yIndex * 0.6f, 0f);

        Instantiate(catPrefab, pos, Quaternion.identity);
        catCount++;

        if (catCount >= maxCats)
        {
            if (spawnButton != null) spawnButton.interactable = false;
            return;
        }

        isCooldown = true;
        timer = 0f;
        if (image != null) image.fillAmount = 0f;
        if (spawnButton != null) spawnButton.interactable = false;
    }
}