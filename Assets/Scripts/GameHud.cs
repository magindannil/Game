using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameHUD : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI[] hearts;
    public TextMeshProUGUI coinsText;

    [Header("Game Settings")]
    public int maxHealth = 3;
    public int currentHealth = 3;
    public int totalCoins = 10;
    public int collectedCoins = 0;

    [Header("Menu")]
    public string menuSceneName = "Menu";

    private void Start()
    {
        UpdateHealthDisplay();
        UpdateCoinsDisplay();
        Debug.Log("GameHUD запущен. Для выхода в меню нажмите Esc");
    }

    private void Update()
    {
        // Более надежная проверка Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    public void UpdateHealthDisplay()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].text = "♥";
                hearts[i].color = Color.red;
            }
            else
            {
                hearts[i].text = "♡";
                hearts[i].color = Color.gray;
            }
        }
    }

    public void UpdateCoinsDisplay()
    {
        coinsText.text = $"Монеты: {collectedCoins} / {totalCoins}";
    }

    public void CollectCoin()
    {
        collectedCoins++;
        UpdateCoinsDisplay();

        if (collectedCoins >= totalCoins)
        {
            Debug.Log("Все монеты собраны!");
        }
    }

    public void ReturnToMenu()
    {
        Debug.Log("Нажата клавиша Esc - возврат в главное меню");

        // Проверяем существует ли сцена
        if (Application.CanStreamedLevelBeLoaded(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogError($"Сцена '{menuSceneName}' не найдена в Build Settings!");

            // Показываем доступные сцены
            Debug.Log("Доступные сцены:");
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                Debug.Log($"- {sceneName} (индекс: {i})");
            }
        }
    }
}