// Coin.cs
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
            CollectCoin(other.gameObject);
        }
    }

    private void CollectCoin(GameObject player)
    {
        // �������� HUD � ��������� ������
        GameHUD hud = FindObjectOfType<GameHUD>();
        if (hud != null)
        {
            hud.CollectCoin();
        }

        // ������ �����
        if (collectEffect != null)
        {
            Instantiate(collectEffect, transform.position, transform.rotation);
        }

        // ���� ����� (����� ��������)
        // AudioSource.PlayClipAtPoint(collectSound, transform.position);

        // ���������� ������
        Destroy(gameObject);

        Debug.Log($"������ �������! �����: {hud.collectedCoins}");
    }
}