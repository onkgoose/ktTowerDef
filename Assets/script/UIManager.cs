using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("экраны")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject victoryPopup;
    [SerializeField] private GameObject defeatPopup;

    [Header("индикаторы")]
    [SerializeField] private TextMeshProUGUI fishText;
    [SerializeField] private TextMeshProUGUI kittensText;
    [SerializeField] private TextMeshProUGUI warriorsText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("найм")]
    [SerializeField] private Button hireKittenButton;
    [SerializeField] private Button hireWarriorButton;

    [Header("итоги")]
    [SerializeField] private TextMeshProUGUI victoryWavesText;
    [SerializeField] private TextMeshProUGUI defeatWavesText;
    [SerializeField] private TextMeshProUGUI notificationText;

    private bool isPaused = false;

    // Переключение экранов
    public void ShowStartScreen()
    {
        HideAllScreens();
        startScreen.SetActive(true);
    }

    public void ShowGameScreen()
    {
        HideAllScreens();
        gameScreen.SetActive(true);
    }

    public void ShowVictoryScreen(int wavesCompleted)
    {
        victoryPopup.SetActive(true);
        if (victoryWavesText != null)
            victoryWavesText.text = $"win!\nwaves complete: {wavesCompleted}";
    }

    public void ShowDefeatScreen(int wavesSurvived)
    {
        defeatPopup.SetActive(true);
        if (defeatWavesText != null)
            defeatWavesText.text = $"game over...\nwaves complete: {wavesSurvived}";
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseScreen.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 0f;
        HideAllScreens();
        startScreen.SetActive(true);
    }

    private void HideAllScreens()
    {
        if (startScreen) startScreen.SetActive(false);
        if (gameScreen) gameScreen.SetActive(false);
        if (pauseScreen) pauseScreen.SetActive(false);
        if (victoryPopup) victoryPopup.SetActive(false);
        if (defeatPopup) defeatPopup.SetActive(false);
    }

    // upd
    public void UpdateResourceLabels(int fish, int kittens, int warriors, int kQueue, int wQueue)
    {
        if (fishText) fishText.text = $" {fish}";
        if (kittensText) kittensText.text = $"{kittens} {(kQueue > 0 ? $"(+{kQueue})" : "")}";
        if (warriorsText) warriorsText.text = $"{warriors} {(wQueue > 0 ? $"(+{wQueue})" : "")}";
    }

    public void UpdateWaveInfo(int wave, int enemies)
    {
        if (waveText) waveText.text = $"wave: {wave}";
        if (enemiesText) enemiesText.text = $"enimies: {enemies}";
    }

    public void UpdateAttackTimer(float secondsRemaining)
    {
        if (timerText) timerText.text = $"{Mathf.CeilToInt(secondsRemaining)}с";
    }

    public void SetKittenButtonInteractable(bool interactable)
    {
        if (hireKittenButton) hireKittenButton.interactable = interactable;
    }

    public void SetWarriorButtonInteractable(bool interactable)
    {
        if (hireWarriorButton) hireWarriorButton.interactable = interactable;
    }

    public void ShowNotification(string message)
    {
        if (notificationText != null)
        {
            StopCoroutine(nameof(ClearNotification));
            notificationText.text = message;
            StartCoroutine(ClearNotification());
        }
    }

    private System.Collections.IEnumerator ClearNotification()
    {
        yield return new WaitForSecondsRealtime(4f);
        if (notificationText != null) notificationText.text = "";
    }
}
