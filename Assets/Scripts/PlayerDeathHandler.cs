using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Settings")]
    public float respawnTime = 1f;
    public int maxLives = 3;

    [Header("Menu Scene")]
    public string menuSceneName = "Menu";

    private bool isDead = false;
    private Rigidbody rb;
    private PlayerController playerController;
    private Vector3 startPosition;
    private int currentLives;
    private GameHUD gameHUD;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        startPosition = transform.position;
        currentLives = maxLives;

        gameHUD = FindObjectOfType<GameHUD>();
        Debug.Log($"Игрок инициализирован. Жизней: {currentLives}/{maxLives}");
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // Уменьшаем жизни
        currentLives--;
        Debug.Log($"Игрок умер. Осталось жизней: {currentLives}");

        // Обновляем HUD если он есть
        if (gameHUD != null)
        {
            // Синхронизируем здоровье в HUD с нашими жизнями
            gameHUD.currentHealth = currentLives;
            gameHUD.UpdateHealthDisplay();
        }

        // Проверяем Game Over
        if (currentLives <= 0)
        {
            GameOver();
            return;
        }

        // Респавн
        if (playerController != null) playerController.enabled = false;
        if (rb != null) rb.linearVelocity = Vector3.zero;

        SetPlayerVisible(false);
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        transform.position = startPosition;

        if (playerController != null) playerController.enabled = true;
        SetPlayerVisible(true);
        isDead = false;

        Debug.Log($"Респавн завершен. Осталось жизней: {currentLives}");
    }

    private void GameOver()
    {
        Debug.Log($"GAME OVER! Все {maxLives} жизни закончились. Переход в меню...");

        // Отключаем управление
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // Скрываем игрока
        SetPlayerVisible(false);

        // Останавливаем физику
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Переходим в меню через 2 секунды
        Invoke("GoToMenu", 2f);
    }

    private void GoToMenu()
    {
        Debug.Log($"Загрузка сцены: {menuSceneName}");
        SceneManager.LoadScene(menuSceneName);
    }

    private void SetPlayerVisible(bool visible)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = visible;
    }

}