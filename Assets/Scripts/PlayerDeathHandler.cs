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
        Debug.Log($"����� ���������������. ������: {currentLives}/{maxLives}");
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        // ��������� �����
        currentLives--;
        Debug.Log($"����� ����. �������� ������: {currentLives}");

        // ��������� HUD ���� �� ����
        if (gameHUD != null)
        {
            // �������������� �������� � HUD � ������ �������
            gameHUD.currentHealth = currentLives;
            gameHUD.UpdateHealthDisplay();
        }

        // ��������� Game Over
        if (currentLives <= 0)
        {
            GameOver();
            return;
        }

        // �������
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

        Debug.Log($"������� ��������. �������� ������: {currentLives}");
    }

    private void GameOver()
    {
        Debug.Log($"GAME OVER! ��� {maxLives} ����� �����������. ������� � ����...");

        // ��������� ����������
        if (playerController != null)
        {
            playerController.enabled = false;
        }

        // �������� ������
        SetPlayerVisible(false);

        // ������������� ������
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // ��������� � ���� ����� 2 �������
        Invoke("GoToMenu", 2f);
    }

    private void GoToMenu()
    {
        Debug.Log($"�������� �����: {menuSceneName}");
        SceneManager.LoadScene(menuSceneName);
    }

    private void SetPlayerVisible(bool visible)
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = visible;
    }

}