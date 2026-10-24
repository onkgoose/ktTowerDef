using UnityEngine;

public class Start : MonoBehaviour
{
    public GameObject startScreen;
    public void HideTarget()
    {
        if (startScreen != null)
        {
            startScreen.SetActive(false);
        }
    }
}
