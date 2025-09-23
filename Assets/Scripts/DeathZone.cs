using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Vector3 respawnPosition;

    private void Start()
    {
        // Находим игрока и запоминаем его начальную позицию
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            respawnPosition = player.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = respawnPosition;
        }
    }
}