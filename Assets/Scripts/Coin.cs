// Coin.cs
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public float rotationSpeed = 60f;
    public GameObject collectEffect;

    private void Update()
    {
        // Вращение монеты
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin(other.gameObject);
        }
    }

    private void CollectCoin(GameObject player)
    {
        // Получаем HUD и добавляем монету
        GameHUD hud = FindObjectOfType<GameHUD>();
        if (hud != null)
        {
            hud.CollectCoin();
        }

        // Эффект сбора
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, transform.rotation);
        }

        // Звук сбора (можно добавить)
        // AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // Уничтожаем монету
        Destroy(gameObject);

        Debug.Log($"Монета собрана! Всего: {hud.collectedCoins}");
    }
}