using UnityEngine;

public class SquashEffect : MonoBehaviour
{
    
    [Tooltip("сила (0.1 = 10%)")]
    public float squashAmount = 0.1f;

    [Tooltip("скорость")]
    public float speed = 2f;

    private Vector3 originalScale;

    void Start()
    {
        Fish.totalFish = 0;
        originalScale = transform.localScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; 

        float yScale = 1f - squashAmount * t;

        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * yScale,
            originalScale.z
        );
    }
}