using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public float rotationSpeed = 60f;
    public GameObject collectEffect;

    private void Update()
    {
        // �������� ������
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        Debug.Log("=== COIN COLLECT ===");

        // ������ ����� - �� ����������� ������!
        if (collectEffect != null)
        {
            Debug.Log("Creating collect effect...");
            // ���������� Quaternion.identity ������ transform.rotation
            Instantiate(collectEffect, transform.position, Quaternion.identity);
            Debug.Log("Effect created at position: " + transform.position);
        }
        else
        {
            Debug.LogError("CollectEffect is NULL! Assign in inspector.");
        }

        // �������� HUD � ��������� ������
        GameHUD hud = FindObjectOfType<GameHUD>();
        if (hud != null)
        {
            hud.CollectCoin();
            Debug.Log($"������ �������! �����: {hud.collectedCoins}");
        }
        else
        {
            Debug.LogWarning("HUD not found!");
        }

        // ���������� ������
        Destroy(gameObject);
    }
}